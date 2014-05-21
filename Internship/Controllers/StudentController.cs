using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Internship.Models.DAL;
using Internship.Models.Domain;
using Internship.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Internship.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {

        private IBedrijfRepository bedrijfRepository;
        private IStudentRepository studentRepository;
        private IStagebegeleiderRepository stagebegeleiderRepository;
        private IUserRepository userRepository;
        private ISpecialisatieRepository specialisatieRepository;
        private IOpdrachtRepository opdrachtRepository;
        private IGemeenteRepository gemeenteRepository;
        private IStatusRepository statusRepository;

        public StudentController(IBedrijfRepository bedrijfR, IStudentRepository studentR,
            IStagebegeleiderRepository stagebegeleiderR, IUserRepository usersRepository,
            ISpecialisatieRepository specialisatie, IOpdrachtRepository opdracht, IGemeenteRepository gemeenteRepository,
            IStatusRepository statusRepository)
        {
            this.bedrijfRepository = bedrijfR;
            this.stagebegeleiderRepository = stagebegeleiderR;
            this.studentRepository = studentR;
            this.userRepository = usersRepository;
            this.specialisatieRepository = specialisatie;
            this.opdrachtRepository = opdracht;
            this.gemeenteRepository = gemeenteRepository;
            this.statusRepository = statusRepository;
        }

        //
        // GET: /Student/
        public ActionResult Index(String id, int? page, string search = null)
        {
            Student student = FindStudent(id);
            IList<Opdracht> opdrachten = opdrachtRepository.GeefStageOpdrachten().ToList();
            if (Request.IsAjaxRequest())
            {
                if (search != null)
                {
                    ViewBag.Selection = "Resultaten voor: " + search;
                    opdrachten = opdrachtRepository.GeefStageOpdrachtenMetZoekstring(search).ToList();
                }
                return PartialView("_StagesPart", opdrachten.ToPagedList(page ?? 1, 12));
            }

            return View(opdrachten.ToPagedList(page ?? 1, 12));
        }

        public ActionResult GetOpdrachtDetail(int id)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            return View(opdracht);

        }

        [HttpPost]
        public ActionResult AddToFavorites(int id)
        {
            //Student student = FindStudent(id);
            Opdracht opr = opdrachtRepository.FindOpdracht(id);
            Student student = FindStudent(User.Identity.GetUserId());
            String message = null;
            if (student.Favorites.Contains(opr))
            {
                message = opr.Title + " was reeds eerder toegevoegd aan favorieten";
            }
            else
            {
                student.AddOpdrachtToFavorites(opr);
                studentRepository.SaveChanges();
                message = opr.Title + " werd toegevoegd aan favorieten";
            }


            if (Request.IsAjaxRequest())
            {
                return Content(message);
            }
            ViewBag["info"] = message;
            return RedirectToAction("Index", student);
        }

        public ActionResult GetFavorites(String id, int? page)
        {
            Student student = FindStudent(id);
            return View("Favoites", student.Favorites.ToList().ToPagedList(page ?? 1, 12));
        }

        public Student FindStudent(String id)
        {
            return studentRepository.FindById(id);
        }

        public ActionResult GetMyIntern(String id)
        {
            Student student = FindStudent(id);
            ViewBag.Title = "Mijnstage";
            return View("GetOpdrachtDetail", student.StageOpdracht);
        }

        public ActionResult GetProfile(String id)
        {
            Student student = FindStudent(id);
            return View(student);
        }

        public ActionResult EditProfile(String id)
        {
            Student student = FindStudent(id);
            RegistratieModelStudent model;
           

            if (student.Adres != null)
            {
                model = new RegistratieModelStudent()
                {
                    Naam = student.Naam,
                    Voornaam = student.Voornaam,
                    GsmNummer = student.Gsmnummer,
                    Straat = student.Adres.StraatNaam,
                    Straatnummer = student.Adres.Nummer,
                    Gemeente = student.Adres.Gemeente.Structuur,

                    Image = student.ImageUrl
                };
            }
            else
            {
                model = new RegistratieModelStudent()
                {
                    Naam = student.Naam,
                    Voornaam = student.Voornaam,
                    GsmNummer = student.Gsmnummer,
                    //Straat = student.Adres.StraatNaam,
                    //Straatnummer = student.Adres.Nummer,
                    //Gemeente = student.Adres.Gemeente.Structuur,
           
                    Image = student.ImageUrl
                };
            }
            if (student.Gebdatum != null)
            {
               DateTime gebD = DateTime.Parse(student.Gebdatum);
                model.GeboorteDatum = gebD;
            }
            RegistratieModelCreater creater = new RegistratieModelCreater(gemeenteRepository, model);
            return View(creater);
        }

        [HttpPost]
        public ActionResult EditProfile([Bind(Prefix = "ModelStudent")] RegistratieModelStudent model, String id,
            HttpPostedFileBase file)
        {
            Student student = FindStudent(id);
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (file.FileName.EndsWith(".jpg") || file.FileName.EndsWith(".png") ||
                        file.FileName.EndsWith(".jpeg"))
                    {
                        file.SaveAs(HttpContext.Server.MapPath("/Images/profiel/") +student.Id+ file.FileName);
                        student.ImageUrl = "/Images/profiel/" +student.Id +file.FileName;
                    }
                    else
                    {
                        TempData["Message"] = "Ongeldig datatype, kies een nieuwe foto met extensie .png of .jpg";
                        return RedirectToAction("EditProfile", model);
                    }

                }
                student.Adres = MakeAdres(model.Straat, model.Straatnummer, model.Gemeente);
                student.Naam = model.Naam;
                student.Voornaam = model.Voornaam;
                student.Gsmnummer = model.GsmNummer;
                if (model.GeboorteDatum!=null)
                {
                    student.Gebdatum = model.GeboorteDatum.ToString();
                }

                studentRepository.SaveChanges();
                TempData["Info"] = "Gegevens zijn gewijzigd";
                return RedirectToAction("GetProfile", student);

            }
            RegistratieModelCreater creater = new RegistratieModelCreater(gemeenteRepository,model);
            return RedirectToAction("EditProfile", creater);

        }

        public Adres MakeAdres(String straat, int nummer, String gemeente)
        {
                return new Adres()
                {
                    Gemeente = gemeenteRepository.FindGemeenteWithStructuur(gemeente),
                    StraatNaam = straat,

                    Nummer = nummer

                };
            
        
        }
        public ActionResult ChangePassword(String id)
        {
            
            return View(new ManageModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(String id, ManageModel model)
        {

            if (ModelState.IsValid)
            {
                IdentityResult result = await userRepository.ChangePaswordAsync(id, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    TempData["Info"] = "Wachtwoord werd gewijzigd";
                    return View("GetProfile", id);
                }
                else
                {
                    TempData["Message"] = "Paswoord niet correct";
                    return RedirectToAction("ChangePassword", model);
                }
            }


            return RedirectToAction("ChangePassword", model);

        }

        public ActionResult GetMyStageForm()
        {
            StudentAddOpdrachtModel model = new StudentAddOpdrachtModel();
            return PartialView("_MyStageForm", model);
        }

        [HttpPost]
        public ActionResult TakeStage(StudentAddOpdrachtModel model,String id)
        {
            Student student = FindStudent(id);
            if (ModelState.IsValid)
            {
                Opdracht opdracht = opdrachtRepository.SearchOpdracht(model.OpdrachtTitle, model.BedrijfsNaam,
                    model.Ondertekenaar, model.Specialisatie);
                if (opdracht == null)
                {
                    TempData["Message"] =
                        "Opdracht werd niet teruggevonden,controleer alle gegevens. Zorg dat alles correct werd geschreven. " +
                        "Als dit bericht na enkele keren terugkomt neem dan contact op met de stage administratie.";
                    return RedirectToAction("GetMyIntern", student);
                }
                if (opdracht.IsOpdrachtFull())
                {
                    TempData["Message"] =
                        "Deze opdracht werd volledig ingenomen, ben je zeker dat je gegevens correct zijn?" +
                        "Indien u een ondertekend stagecontract heeft, neem contact op met de stage administratie";
                    return RedirectToAction("GetMyIntern", student);
                }
                opdracht.AddStudent(student);
                if (opdracht.IsOpdrachtFull())
                {
                    opdracht.Status = statusRepository.FindStatusWithId(6);
                }
                else
                {
                    opdracht.Status = statusRepository.FindStatusWithId(5);
                }
            opdrachtRepository.SaveChanges();
                return RedirectToAction("GetMyIntern", student);

            }

            return RedirectToAction("GetMyIntern", student);
        }
    }
   



}