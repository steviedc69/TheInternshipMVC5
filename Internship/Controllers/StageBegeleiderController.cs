using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Internship.Models.Domain;
using Internship.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Ninject;
using PagedList;

namespace Internship.Controllers
{
    public class StageBegeleiderController : Controller
    {

        private IBedrijfRepository bedrijfRepository;
        private IStudentRepository studentRepository;
        private IStagebegeleiderRepository stagebegeleiderRepository;
        private IUserRepository userRepository;
        private ISpecialisatieRepository specialisatieRepository;
        private IOpdrachtRepository opdrachtRepository;
        private IGemeenteRepository gemeenteRepository;

        public StageBegeleiderController(IBedrijfRepository bedrijfR, IStudentRepository studentR,
            IStagebegeleiderRepository stagebegeleiderR, IUserRepository usersRepository,
            ISpecialisatieRepository specialisatie, IOpdrachtRepository opdracht, IGemeenteRepository gemeenteRepository)
        {
            this.bedrijfRepository = bedrijfR;
            this.stagebegeleiderRepository = stagebegeleiderR;
            this.studentRepository = studentR;
            this.userRepository = usersRepository;
            this.specialisatieRepository = specialisatie;
            this.opdrachtRepository = opdracht;
            this.gemeenteRepository = gemeenteRepository;
        }

        //
        // GET: /StageBegeleider/
        public ActionResult Index(String id, int? page)
        {
            IList<Opdracht> stageopdrachten = opdrachtRepository.GeefActieveOpdrachten().ToList();
            return View(stageopdrachten.ToPagedList(page ?? 1, 12));
        }

        public Stagebegeleider FindStagebegeleider(String id)
        {
            return stagebegeleiderRepository.FindById(id);
        }

        public ActionResult GetOpdrachtDetails(int id)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            return View("GetOpdrachtDetail", opdracht);
        }

        [HttpPost]
        public ActionResult AddToFavorites(int id)
        {
            //Student student = FindStudent(id);
            Opdracht opr = opdrachtRepository.FindOpdracht(id);
            Stagebegeleider stbg = FindStagebegeleider(User.Identity.GetUserId());
            String message = null;
            if (stbg.Preferences.Contains(opr))
            {
                message = opr.Title + " was reeds eerder toegevoegd aan favorieten";
            }
            else
            {
                stbg.AddToPreferences(opr);
                stagebegeleiderRepository.SaveChanges();
                message = opr.Title + " werd toegevoegd aan favorieten";
            }


            if (Request.IsAjaxRequest())
            {
                return Content(message);
            }
            ViewBag["info"] = message;
            return RedirectToAction("Index", stbg);
        }

        public ActionResult GetFavorites(String id, int? page)
        {
            Stagebegeleider begeleider = FindStagebegeleider(id);
            return View(begeleider.Preferences.ToList().ToPagedList(page ?? 1, 12));
        }

        public ActionResult GetTeBegeleidenStages(String id)
        {
            Stagebegeleider begeleider = FindStagebegeleider(id);
            return View(begeleider);
        }

        public ActionResult GetTeBegeleidenStageDetail(int id)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_StageFirstPartial", opdracht);
            }
            return View("GetOpdrachtDetail", opdracht);
        }

        public ActionResult GetSortBy(String id, String select = null)
        {
            Stagebegeleider begeleider = FindStagebegeleider(id);
            return PartialView("_StagesList", SortHelper(begeleider, select));

        }

        public IList<Opdracht> SortHelper(Stagebegeleider begeleider, String select = null)
        {
            if (select == null||select.Equals("Schooljaar"))
            {
                begeleider.AddSort(new SortBySchoojaar(begeleider.TeBegeleidenOpdrachten.ToList()));
               
            }
            else
            {
                if (select.Equals("Bedrijf"))
                {
                    begeleider.AddSort(new SortByBedrijf(begeleider.TeBegeleidenOpdrachten.ToList()));
                }
                if (select.Equals("Titel"))
                {
                    begeleider.AddSort(new SortByTitle(begeleider.TeBegeleidenOpdrachten.ToList()));
                }
                if (select.Equals("Specialisatie"))
                {
                    begeleider.AddSort(new SortBySpecialisatie(begeleider.TeBegeleidenOpdrachten.ToList()));
                }
                if (select.Equals("Dit schooljaar"))
                {
                    begeleider.AddSort(new SortByDitSchooljaar(begeleider.TeBegeleidenOpdrachten.ToList()));
                }
            }
            return begeleider.Sorteer();
        }

        public ActionResult GetTeBegeleidenStudenten(String id)
        {
            Stagebegeleider begeleider = FindStagebegeleider(id);

            return View(begeleider);
        }

        public ActionResult GetTeBegeleidenStageStudentDetail(String id)
        {
            Student student = studentRepository.FindById(id);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_StudentFirstPartial", student);
            }
            return View("GetOpdrachtDetail", student.StageOpdracht);
        }

        public ActionResult GetProfile(String id)
        {
            Stagebegeleider begeleider = FindStagebegeleider(id);
            return View(begeleider);
        }

        public ActionResult EditProfile(String id)
        {
            Stagebegeleider begeleider = FindStagebegeleider(id);
            StagebegeleiderModel model = new StagebegeleiderModel()
            {
                Naam = begeleider.Naam,
                Voornaam = begeleider.Voornaam,
                GsmNummer = begeleider.Gsmnummer
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditProfile(StagebegeleiderModel model,String id,String button)
        {
            Stagebegeleider begeleider = FindStagebegeleider(id);
            if (button.Equals("annuleer"))
            {
                return RedirectToAction("EditProfile", begeleider);
            }
            if (ModelState.IsValid)
            {
                begeleider.Naam = model.Naam;
                begeleider.Voornaam = model.Voornaam;
                begeleider.Gsmnummer = model.GsmNummer;
                stagebegeleiderRepository.SaveChanges();
                TempData["Info"] = "Gegevens werden gewijzigd";
                return RedirectToAction("GetProfile", begeleider);
            }
            return RedirectToAction("EditProfile", model);
        }

        public ActionResult ChangePassword(String id)
        {
            Stagebegeleider begeleider = FindStagebegeleider(id);
            return View(new ManageModel());
        }

            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(String id, ManageModel model)
            {
                Stagebegeleider begeleider = FindStagebegeleider(id);
            if (ModelState.IsValid)
            {
                IdentityResult result = await userRepository.ChangePaswordAsync(id, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    TempData["Info"] = "Wachtwoord werd gewijzigd";
                    return View("GetProfile", begeleider);
                }
                else
                {
                    TempData["Message"] = "Paswoord niet correct";
                    return RedirectToAction("ChangePassword", model);
                }
            }
               return RedirectToAction("ChangePassword", model);

        }
    }
}
