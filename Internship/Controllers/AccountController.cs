using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Internship.Models.DAL;
using Internship.Models.Domain;
using Internship.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Internship.Models;

namespace Internship.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IBedrijfRepository BedrijfRepository;
        private IStudentRepository StudentRepository;
        private IStagebegeleiderRepository StagebegeleiderRepository;
        private IUserRepository UserRepository;
        private IGemeenteRepository gemeenteRepository;


        public AccountController(IBedrijfRepository bedrijfRepository, IStudentRepository studentRepository,
            IStagebegeleiderRepository stagebegeleider, IUserRepository userRepository,IGemeenteRepository gemeenteRepository)
        {
           
            this.BedrijfRepository = bedrijfRepository;
            this.StagebegeleiderRepository = StagebegeleiderRepository;
            this.StudentRepository = studentRepository;
            this.UserRepository = userRepository;
            this.gemeenteRepository = gemeenteRepository;

        }

        //public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (model.Email.EndsWith("@student.hogent.be"))
                {
                    DomainFactory.CreateNewStudent(model.Email,UserRepository,StudentRepository);
                }
                var user = await UserRepository.FindAsyncUser(model.Email, model.Passwd);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    if (user.UserName.EndsWith("@student.hogent.be"))
                    {
                        Student student = user as Student;
                        if (!student.NotFirstTime)
                        {
                            return RedirectToAction("Manage");
                        }
                        else
                        {
                           return RedirectToAction("Index","Student");
                        }
                    }
                    Bedrijf b = BedrijfRepository.FindByEmail(user.UserName);
                    if (b !=null)
                    {
                        return RedirectToAction("Index", "Bedrijf", b);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            RegistratieModelCreater creater = new RegistratieModelCreater(gemeenteRepository,new RegistratieModel());
            return View(creater);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Prefix = "RegistratieModel")]RegistratieModel model)
        {
            if (ModelState.IsValid)
            {

                var user = DomainFactory.createBedrijf(model.Bedrijfsnaam, model.Activiteit, model.Bereikbaarheid,
                    model.Url,
                    model.Straat, model.Straatnummer, model.Woonplaats, model.Telefoon, model.Email,gemeenteRepository);
                var result = await UserRepository.CreateAsyncUser(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);
                    Bedrijf b = user;
                    return RedirectToAction("Index", "Bedrijf",b);
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(new RegistratieModelCreater(gemeenteRepository,model));
        }


        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserRepository.ChangePaswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        if (User.Identity.GetUserName().EndsWith("@student.hogent.be"))
                        {
                            StudentRepository.UpdateFirstTime(User.Identity.GetUserName(), true);
                            return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });

                        }
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserRepository.AddAsyncPassword(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                       
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        
   

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

 

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserRepository.Manager != null)
            {
                UserRepository.Manager.Dispose();
                UserRepository.Manager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserRepository.Manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserRepository.Manager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}