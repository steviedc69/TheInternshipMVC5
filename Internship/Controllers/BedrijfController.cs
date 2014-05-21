using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Internship.Models.DAL;
using Internship.Models.Domain;
using Internship.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace Internship.Controllers
{
    [Authorize]
    public class BedrijfController : Controller
    {
        private IBedrijfRepository bedrijfRepository;
        private IStudentRepository studentRepository;
        private IStagebegeleiderRepository stagebegeleiderRepository;
        private IUserRepository userRepository;
        private ISpecialisatieRepository specialisatieRepository;
        private IOpdrachtRepository opdrachtRepository;
        private IGemeenteRepository gemeenteRepository;
        private IContactPersoonRepository contactPersoonRepository;
        private IStatusRepository statusRepository;

        //public UserController(){}

        public BedrijfController(IBedrijfRepository bedrijfR, IStudentRepository studentR,
            IStagebegeleiderRepository stagebegeleiderR, IUserRepository usersRepository,
            ISpecialisatieRepository specialisatie, IOpdrachtRepository opdracht,
            IContactPersoonRepository contactPersoonRepository,
            IGemeenteRepository gemeenteRepository, IStatusRepository statusRepository
            )
        {
            this.bedrijfRepository = bedrijfR;
            this.stagebegeleiderRepository = stagebegeleiderR;
            this.studentRepository = studentR;
            this.userRepository = usersRepository;
            this.specialisatieRepository = specialisatie;
            this.opdrachtRepository = opdracht;
            this.gemeenteRepository = gemeenteRepository;
            this.contactPersoonRepository = contactPersoonRepository;
            this.statusRepository = statusRepository;
        }

        //
        // GET: /Bedrijf/
        public ActionResult Index(String id, int? page,String search = null,String select = null)
        {
            Bedrijf b = bedrijfRepository.FindById(id);
            ViewBag.Title = b.Bedrijfsnaam;
            ViewBag.BedrijfId = b.Id;
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_OpdrachtenListPartial",
                    SearchHelper(b,search,select).ToPagedList(page ?? 1, 5));
            }


            return View(b.GetListOfActiveOpdrachten(year,month).ToPagedList(page ?? 1, 5));
        }

        public IList<Opdracht> SearchHelper(Bedrijf bedrijf,String search = null, String select = null)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            if (select != null)
            {
                if (select.Equals("Alle opdrachten"))
                {
                    bedrijf.AddStrategy(new SearchByAll());
                    return bedrijf.SearchResult(bedrijf.GetListOfActiveOpdrachten(year, month), search);
                }
                else if (select.Equals("Uit archief"))
                {
                    bedrijf.AddStrategy(new SearchByAll());
                    return bedrijf.SearchResult(bedrijf.GetArchive(year), search);
                }
                else if (select.Equals("per contactpersoon"))
                {
                    bedrijf.AddStrategy(new SearchByContact());
                    return bedrijf.SearchResult(bedrijf.Opdrachten.ToList(), search);
                }
                else if (select.Equals("per schooljaar"))
                {
                    bedrijf.AddStrategy(new SearchBySchooljaar());
                    return bedrijf.SearchResult(bedrijf.Opdrachten.ToList(), search);
                }
                else if (select.Equals("uit goedgekeurde opdrachten"))
                {
                    bedrijf.AddStrategy(new SearchByStatus(3));
                    return bedrijf.SearchResult(bedrijf.Opdrachten.ToList(), search);
                }
                else if (select.Equals("uit pending opdrachten"))
                {
                    bedrijf.AddStrategy(new SearchByStatus(1));
                    return bedrijf.SearchResult(bedrijf.Opdrachten.ToList(), search);
                }
                else if (select.Equals("uit afgekeurde opdrachten"))
                {
                    bedrijf.AddStrategy(new SearchByStatus(2));
                    return bedrijf.SearchResult(bedrijf.Opdrachten.ToList(), search);  
                }
                else if (select.Equals("op specialisatie"))
                {
                    bedrijf.AddStrategy(new SearchByStatus.SearchBySpecialisatie());
                    return bedrijf.SearchResult(bedrijf.Opdrachten.ToList(), search);
                }
                else
                {
                    if (search == null)
                    {
                        return bedrijf.GetListOfActiveOpdrachten(year, month).OrderBy(o => o.Schooljaar).ToList();
                    }
                    else
                    {
                        bedrijf.AddStrategy(new SearchByAll());
                        return bedrijf.SearchResult(bedrijf.Opdrachten.ToList(), search);
                    }
                }
            }
            else
            {
                if (search == null)
                {
                    return bedrijf.GetListOfActiveOpdrachten(year, month).OrderBy(o => o.Schooljaar).ToList();
                }
                else
                {
                    bedrijf.AddStrategy(new SearchByAll());
                    return bedrijf.SearchResult(bedrijf.Opdrachten.ToList(), search);
                }
            }
        }

        public ActionResult AddOpdracht(String id = null, ContactModel modelContact = null)
        {
            IEnumerable<Specialisatie> specialisaties;
            Bedrijf b = bedrijfRepository.FindById(id);
            specialisaties = specialisatieRepository.FindAllSpecialisaties();
            CreateOpdrachtViewModel model = new CreateOpdrachtViewModel(specialisaties, b.ContactPersonen,
                new OpdrachtViewModel()
                {
                    Straat = b.Adres.StraatNaam,
                    Nummer = b.Adres.Nummer,
                    Gemeente = b.Adres.Gemeente.Structuur,
                    IsBedrijfAdres = true
                }, id, gemeenteRepository);
            return View(model);
        }

        public ActionResult EditOpdracht(int id)
        {
            IEnumerable<Specialisatie> specialisaties;
            Bedrijf b = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            Opdracht o = opdrachtRepository.FindOpdracht(id);
            if (o == null)
            {
                return HttpNotFound();
            }
            specialisaties = specialisatieRepository.FindAllSpecialisaties();
            CreateOpdrachtViewModel opdrachtView = new CreateOpdrachtViewModel(specialisaties, b.ContactPersonen,
                new OpdrachtViewModel(), b.Id, gemeenteRepository);
            opdrachtView.Opdracht = o;
            opdrachtView.FillOpdrachtView();

            return View("AddOpdracht", opdrachtView);
        }

        [HttpPost]
        public ActionResult EditOpdracht([Bind(Prefix = "OpdrachtViewModel")] OpdrachtViewModel model, int id,
            String button)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            Bedrijf b = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            if (opdracht == null)
            {
                TempData["Message"] = "Opdracht niet gevonden";
                return RedirectToAction("Index", "Bedrijf", b);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    ViewModelToOpdracht(model, opdracht, b);
                    TempData["Info"] = "Opdracht " + opdracht.Title + " werd aangepast";
                    if (button.Equals("contact"))
                    {
                        return RedirectToAction("AddContactToOpdracht", "Bedrijf", opdracht);
                    }
                    else
                    {
                        return RedirectToAction("OpdrachtDetail", opdracht);
                    }

                }
                catch (Exception e)
                {

                    ModelState.AddModelError("", e.Message);
                }

            }

            IEnumerable<Specialisatie> specialisaties;
            specialisaties = specialisatieRepository.FindAllSpecialisaties();
            CreateOpdrachtViewModel opdrachtView = new CreateOpdrachtViewModel(specialisaties, b.ContactPersonen,
                new OpdrachtViewModel(), b.Id, gemeenteRepository);
            opdrachtView.Opdracht = opdracht;
            opdrachtView.FillOpdrachtView();
            return View("AddOpdracht", opdrachtView);

        }

        private void ViewModelToOpdracht(OpdrachtViewModel model, Opdracht opdracht, Bedrijf b)
        {
            if (model.Semesters.Equals("Semester 1"))
            {
                opdracht.IsSemester1 = true;
                opdracht.IsSemester2 = false;
            }
            else if (model.Semesters.Equals("Semester 2"))
            {
                opdracht.IsSemester2 = true;
                opdracht.IsSemester1 = false;
            }
            else
            {
                opdracht.IsSemester1 = true;
                opdracht.IsSemester2 = true;
            }

            if (model.IsBedrijfAdres)
            {
                opdracht.Adres = b.Adres;
            }
            else
            {
                opdracht.Adres = new Adres()
                {
                    StraatNaam = model.Straat,
                    Nummer = (int) model.Nummer,
                    Gemeente = gemeenteRepository.FindGemeenteWithStructuur(model.Gemeente)
                };
            }
            opdracht.Title = model.Title;
            opdracht.Omschrijving = model.Omschrijving;
            opdracht.Schooljaar = model.Schooljaar;
            opdracht.Vaardigheden = model.Vaardigheden;
            opdracht.Specialisatie = specialisatieRepository.FindSpecialisatieNaam(model.Schooljaar);
            opdrachtRepository.SaveChanges();
        }

        //
        [HttpPost]
        public ActionResult AddOpdracht([Bind(Prefix = "OpdrachtViewModel")] OpdrachtViewModel model, String id,
            String button)
        {
            IEnumerable<Specialisatie> specialisaties;
            Bedrijf b = bedrijfRepository.FindById(id);
            specialisaties = specialisatieRepository.FindAllSpecialisaties();
            if (ModelState.IsValid)
            {
                Opdracht opdracht = null;
                Bedrijf bedrijf = FindBedrijf(id);
                if (model.IsBedrijfAdres)
                {
                    opdracht = DomainFactory.CreateOpdrachtWhereAdresIsCompanyAdres((int) model.AantalStudenten,
                        model.Schooljaar, model.Semesters,
                        model.Title, model.Omschrijving,
                        model.Vaardigheden, model.Specialisatie, bedrijf,
                        specialisatieRepository, gemeenteRepository, statusRepository);
                }
                else
                {
                    if (model.Nummer.HasValue)
                    {
                        opdracht = DomainFactory.CreateOpdrachtWithNewAdres(model.AantalStudenten,
                            model.Schooljaar,
                            model.Semesters, model.Title,
                            model.Omschrijving,
                            model.Vaardigheden, model.Specialisatie, bedrijf,
                            model.Straat, (int) model.Nummer
                            , model.Gemeente, specialisatieRepository,
                            gemeenteRepository, statusRepository);
                    }
                    else
                    {
                        ViewBag.Error = "Nummer veld is verplicht";
                    }

                }
                b.AddOpdracht(opdracht);
                bedrijfRepository.SaveChanges();
                Bewerkingen.SendMail(b.UserName,model);
                if (button.Equals("contact"))
                {
                    TempData["Info"] = "Opdracht " + opdracht.Title + " werd succesvol aangemaakt";
                    return RedirectToAction("AddContactToOpdracht", "Bedrijf", opdracht);
                }
                else
                {
                    TempData["Info"] = "Opdracht " + opdracht.Title + " werd succesvol aangemaakt";
                    return RedirectToAction("OpdrachtDetail", "Bedrijf", opdracht);
                }

            }

            return
                View(new CreateOpdrachtViewModel(specialisaties, b.ContactPersonen, new OpdrachtViewModel(),
                    id, gemeenteRepository));

        }

        public ActionResult BeheerContacten(String id)
        {
            Bedrijf bedrijf = FindBedrijf(id);
            return View(bedrijf);

        }

        public ActionResult AddContact(String id)
        {
            return View(new ContactPersoon().ConvertToContactCreateModel(id));
        }

        public ActionResult AddContactToOpdracht(Opdracht opdracht)
        {
            Bedrijf b = bedrijfRepository.FindBedrijfByOpdrachtId(opdracht.Id);
            CreateContactPersoonView ccp = new CreateContactPersoonView(opdracht, b.ContactPersonen);
            return View(ccp);
        }

        // Contact wordt toegevoegd
        [HttpPost]
        public ActionResult AddContact(ContactModel contact, string id)
        {
            if (ModelState.IsValid)
            {
                FindBedrijf(id)
                    .AddContactPersoon(new ContactPersoon(contact.Naam, contact.Voornaam, contact.Functie,
                        contact.ContactEmail, contact.ContactTelNr, contact.GsmNummer));
                bedrijfRepository.SaveChanges();
                return View("Index");

            }
            return View(contact);
        }

        public Bedrijf FindBedrijf(String id)
        {
            return bedrijfRepository.FindById(id);
        }

        public ActionResult GetContactFromList(int id)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            Bedrijf b = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            CreateContactPersoonView ccpv = new CreateContactPersoonView(opdracht, b.ContactPersonen);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_UitLijst", ccpv);
            }
            return View("Index", b);
        }

        public ActionResult GetStageBegeleiderFromList(int id)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            Bedrijf bedrijf = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            CreateContactPersoonView ccpv = new CreateContactPersoonView(opdracht, bedrijf.ContactPersonen);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_StagebegUitLijst", ccpv);
            }
            return View("_StagebegUitLijst", ccpv);
        }

        public ActionResult GetNewContact(int id)
        {
            ContactModel model = new ContactModel()
            {
                id = id,
                BedrijfsId = bedrijfRepository.FindBedrijfByOpdrachtId(id).Id
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ContactFormPartial", model);
            }
            return View("AddContact", model);
        }

        public ActionResult GetNewStageBegeleider(int id)
        {
            ContactModel model = new ContactModel()
            {
                id = id,
                BedrijfsId = bedrijfRepository.FindBedrijfByOpdrachtId(id).Id
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_StageBegeleiderForm", model);
            }
            return View("AddContact", model);
        }

        [HttpPost]
        public ActionResult MakeContactFromList(
            [Bind(Prefix = "ContactToOpdrachtFromList")] AddContactToOpdrachtView model, int id)
        {

            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            Bedrijf b = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            if (ModelState.IsValid)
            {

                ContactPersoon c = b.FindContactPersoon(model.ContactPersoon);
                opdracht.Ondertekenaar = c;
                opdrachtRepository.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_ContactDetail", c);
                }
            }
            return RedirectToAction("GetContactFromList", opdracht.Id);

        }

        [HttpPost]
        public ActionResult MakeStageBegeleiderFromList(
            [Bind(Prefix = "ContactToOpdrachtFromList")] AddContactToOpdrachtView model, int id)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            Bedrijf b = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            if (ModelState.IsValid)
            {

                ContactPersoon c = b.FindContactPersoon(model.ContactPersoon.Trim());
                opdracht.StageMentor = c;
                opdrachtRepository.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_ContactDetail", c);
                }
            }
            return RedirectToAction("GetStageBegeleiderFromList", opdracht.Id);
        }

        [HttpPost]
        public ActionResult MakeContactFromForm(ContactModel model, int id)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            Bedrijf bedrijf = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            if (ModelState.IsValid)
            {
                ContactPersoon c = new ContactPersoon()
                {
                    ContactEmail = model.ContactEmail.Trim(),
                    ContactTelNr = model.ContactTelNr,
                    Functie = model.Functie.Trim(),
                    GsmNummer = model.GsmNummer,
                    Naam = model.Naam.Trim(),
                    Voornaam = model.Voornaam.Trim()
                };
                bedrijf.AddContactPersoon(c);
                opdracht.Ondertekenaar = c;
                bedrijfRepository.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_ContactDetail", c);
                }
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ContactFormPartial", model);
            }
            return RedirectToAction("MakeContactFromForm", model);
        }

        [HttpPost]
        public ActionResult MakeStageBegeleiderFromForm(ContactModel model, int id)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            Bedrijf bedrijf = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            if (ModelState.IsValid)
            {
                ContactPersoon c = null;
                ContactPersoon contact = contactPersoonRepository.FindPersoonWithNaamVoornaamEmail(model.Naam, model.Voornaam,
                    model.ContactEmail);
                if (contact!=null)
                {
                    c = contact;
                    c.Actief = true;
                }
                else
                {
                    c = new ContactPersoon()
                    {
                        ContactEmail = model.ContactEmail.Trim(),
                        ContactTelNr = model.ContactTelNr,
                        Functie = model.Functie.Trim(),
                        GsmNummer = model.GsmNummer,
                        Naam = model.Naam.Trim(),
                        Voornaam = model.Voornaam.Trim()
                    };
                    bedrijf.AddContactPersoon(c);
                }
             
                opdracht.StageMentor = c;
                opdrachtRepository.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_ContactDetail", c);
                }
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_StageBegeleiderForm", model);
            }
            return RedirectToAction("GetNewStageBegeleider", model);
        }

        public ActionResult OpdrachtDetail(int id)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);

            return View(opdracht);
        }

        public ActionResult ToRemoveOpdracht(int id)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);

            return View(opdracht);
        }

        [HttpPost]
        public ActionResult RemoveOpdracht(int id, String button)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            Bedrijf bedrijf = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            String title = opdracht.Title;
            if (button.Equals("ja"))
            {

                opdrachtRepository.RemoveOpdracht(opdracht);
                opdrachtRepository.SaveChanges();
                TempData["Info"] = "De opdracht " + title + " werd verwijderd";
                return RedirectToAction("Index", "Bedrijf", bedrijf);
            }
            else
            {
                TempData["Message"] = "De opdracht " + title + " werd niet verwijderd";
                return RedirectToAction("Index", "Bedrijf", bedrijf);
            }
        }

        public ActionResult ToRemoveContact(int id)
        {
            ContactPersoon contact = contactPersoonRepository.FindContactPersoon(id);
            Bedrijf b = bedrijfRepository.FindBedrijfByContactPersId(id);

            return View(contact);

        }

        [HttpPost]
        public ActionResult RemoveContact(int id, String button)
        {
            ContactPersoon contact = contactPersoonRepository.FindContactPersoon(id);
            Bedrijf b = bedrijfRepository.FindBedrijfByContactPersId(id);
            String title = contact.Naam + " " + contact.Voornaam;
          
                if (button.Equals("ja"))
                {
                    if (b.IsOpdrachtenWithContact(contact))
                    {
                        contact.Actief = false;
                        contactPersoonRepository.SaveChanges();
                    }
                    else
                    {
                        contactPersoonRepository.VerwijderContact(contact);
                    }

                    TempData["Info"] = "De contactpersoon, " + title + ", werd verwijderd";
                    return RedirectToAction("BeheerContacten", b);
                }
                else
                {
                    TempData["Message"] = "De contactpersoon " + title + " werd niet verwijderd";
                    return RedirectToAction("BeheerContacten", b);
                }
      


        }

        public ActionResult EditContact(int id)
        {
            ContactPersoon contact = contactPersoonRepository.FindContactPersoon(id);
            Bedrijf bedrijf = bedrijfRepository.FindBedrijfByContactPersId(id);
            if (contact==null)
            {
                TempData["info"] = "Contact niet gevonden";
                return RedirectToAction("BeheerContacten", bedrijf);
            }
            ContactModel model = new ContactModel()
            {
                id = contact.Id,
                ContactEmail = contact.ContactEmail,
                Functie = contact.Functie,
                ContactTelNr = contact.ContactTelNr,
                GsmNummer = contact.GsmNummer,
                Naam = contact.Naam,
                Voornaam = contact.Voornaam
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_EditContact", model);
            }
            return PartialView("_EditContact", model);
        }


        [HttpPost]
        public ActionResult ConfirmEditContact(ContactModel model,int id,String button)
        {
            ContactPersoon persoon = contactPersoonRepository.FindContactPersoon(id);
            Bedrijf b = bedrijfRepository.FindBedrijfByContactPersId(id);

            if (button.Equals("annuleer"))
            {
                return RedirectToAction("BeheerContacten", b);
            }
            if (ModelState.IsValid)
            {
                persoon.Naam = model.Naam;
                persoon.Voornaam = model.Voornaam;
                persoon.GsmNummer = model.GsmNummer;
                persoon.ContactEmail = model.ContactEmail;
                persoon.Functie = model.Functie;
                persoon.ContactTelNr = model.ContactTelNr;
                contactPersoonRepository.SaveChanges();

                TempData["Info"] = "Contact gegevens van " + persoon.Naam + " " + persoon.Voornaam + " werden gewijzigd";
                return RedirectToAction("BeheerContacten", b);
            }

            return RedirectToAction("EditContact",model);

        }

        public ActionResult ShowProfile(String id)
        {
            Bedrijf b = FindBedrijf(id);
            return View(b);

        }

        public ActionResult EditProfile(String id)
        {
            Bedrijf b = FindBedrijf(id);
            UpdateModel updateModel = new UpdateModel()
            {
                Activiteit = b.Activiteit,
                Bedrijfsnaam = b.Bedrijfsnaam,
                Openbaarvervoer = b.Openbaarvervoer,
                PerAuto = b.PerAuto,
                Url = b.Url,
                Straat = b.Adres.StraatNaam,
                Straatnummer = b.Adres.Nummer,
                Telefoon = b.Telefoon,
                Woonplaats = b.Adres.Gemeente.Structuur,
                Image = b.ImageUrl
                
            };
            RegistratieModelCreater model = new RegistratieModelCreater(gemeenteRepository,updateModel);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditProfile([Bind(Prefix = "UpdateModel")] UpdateModel model, String id,HttpPostedFileBase file)
        {
            Bedrijf b = FindBedrijf(id);
            //HttpPostedFileBase file = Request.Files["file"];
            if (ModelState.IsValid)
            {
                if (file!=null)
                {
                    if (file.FileName.EndsWith(".jpg") || file.FileName.EndsWith(".png") ||
                        file.FileName.EndsWith(".jpeg"))
                    {
                        String path = "/Images/logo/";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        file.SaveAs(HttpContext.Server.MapPath(path) +b.Id +file.FileName);
                        b.ImageUrl = path + b.Id+file.FileName;
                    }
                }
                b.Bedrijfsnaam = model.Bedrijfsnaam;
                b.PerAuto = model.PerAuto;
                b.Openbaarvervoer = model.Openbaarvervoer;
                b.Activiteit = model.Activiteit;
                b.Telefoon = model.Telefoon;
                b.Url = model.Url;
                b.Adres.StraatNaam = model.Straat;
                b.Adres.Nummer = model.Straatnummer;
                b.Adres.Gemeente = gemeenteRepository.FindGemeenteWithStructuur(model.Woonplaats);
                bedrijfRepository.SaveChanges();

                TempData["Info"] = "Gegevens werden gewijzigd";
                return RedirectToAction("Index", b);
            }
            RegistratieModelCreater creater = new RegistratieModelCreater(gemeenteRepository,model);
            return RedirectToAction("EditProfile", creater);

        }

        public ActionResult ChangePassword(String id)
        {
            if (Request.IsAjaxRequest())
            {
                
                return PartialView("_ChangePassword",new ManageModel());
                //return RedirectToAction("Manage", "Account");
            }
            return View(new ManageModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(String id,ManageModel model)
        {

            if (ModelState.IsValid)
            {
                IdentityResult result = await userRepository.ChangePaswordAsync(id, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    TempData["Info"] = "Wachtwoord werd gewijzigd";
                    return View("ChangePassword", model);
                }
                else
                {
                    TempData["Message"] = "Paswoord niet correct";
                    return View("ChangePassword",model);
                }
            }


            return View("ChangePassword",model);

        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
}
}
