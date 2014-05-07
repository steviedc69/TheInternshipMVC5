using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Internship.Models.DAL;
using Internship.Models.Domain;
using Internship.ViewModels;
using Microsoft.Ajax.Utilities;
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
            ISpecialisatieRepository specialisatie, IOpdrachtRepository opdracht, IContactPersoonRepository contactPersoonRepository, 
            IGemeenteRepository gemeenteRepository,IStatusRepository statusRepository
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
        public ActionResult Index(String id,int? page)
        {
            Bedrijf b = bedrijfRepository.FindById(id);
            ViewBag.Title = b.Bedrijfsnaam;
            
            ViewBag.BedrijfId = b.Id;
            return View(b.Opdrachten.ToList().ToPagedList(page??1,5));
        }

        public ActionResult AddOpdracht(String id = null, ContactModel modelContact = null)
        {
            IEnumerable<Specialisatie> specialisaties;
            Bedrijf b = bedrijfRepository.FindById(id);
            specialisaties = specialisatieRepository.FindAllSpecialisaties();
            CreateOpdrachtViewModel model = new CreateOpdrachtViewModel(specialisaties, b.ContactPersonen,
                new OpdrachtViewModel(){Straat = b.Adres.StraatNaam,Nummer = b.Adres.Nummer,Gemeente = b.Adres.Gemeente.Structuur,IsBedrijfAdres = true}, id,gemeenteRepository);
            return View(model);
        }

        public ActionResult EditOpdracht(int id)
        {
            IEnumerable<Specialisatie> specialisaties;
            Bedrijf b = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            Opdracht o = opdrachtRepository.FindOpdracht(id);
            if (o==null)
            {
                return HttpNotFound();
            }
            specialisaties = specialisatieRepository.FindAllSpecialisaties();
            CreateOpdrachtViewModel opdrachtView = new CreateOpdrachtViewModel(specialisaties,b.ContactPersonen,new OpdrachtViewModel(),b.Id,gemeenteRepository);
            opdrachtView.Opdracht = o;
            opdrachtView.FillOpdrachtView();

            return View("AddOpdracht",opdrachtView);
        }

        [HttpPost]
        public ActionResult EditOpdracht([Bind(Prefix = "OpdrachtViewModel")]OpdrachtViewModel model,int id,String button)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            Bedrijf b = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            if (opdracht==null)
            {
                TempData["Message"] = "Opdracht niet gevonden";
                return RedirectToAction("Index", "Bedrijf", b);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    ViewModelToOpdracht(model,opdracht,b);
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
                    
                    ModelState.AddModelError("",e.Message);
                }

            }

            IEnumerable<Specialisatie> specialisaties;
            specialisaties = specialisatieRepository.FindAllSpecialisaties();
            CreateOpdrachtViewModel opdrachtView = new CreateOpdrachtViewModel(specialisaties, b.ContactPersonen, new OpdrachtViewModel(), b.Id, gemeenteRepository);
            opdrachtView.Opdracht = opdracht;
            opdrachtView.FillOpdrachtView();
            return View("AddOpdracht", opdrachtView);

        }

        private void ViewModelToOpdracht(OpdrachtViewModel model, Opdracht opdracht,Bedrijf b)
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
                    Nummer =(int)model.Nummer,
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
        public ActionResult AddOpdracht([Bind(Prefix = "OpdrachtViewModel")]OpdrachtViewModel model, String id, String button)
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
                    opdracht = DomainFactory.CreateOpdrachtWhereAdresIsCompanyAdres((int)model.AantalStudenten,
                        model.Schooljaar, model.Semesters,
                        model.Title,model.Omschrijving,
                        model.Vaardigheden,model.Specialisatie,bedrijf,
                        specialisatieRepository,gemeenteRepository,statusRepository);
                }
                else
                {
                    if (model.Nummer.HasValue)
                    {
                        opdracht = DomainFactory.CreateOpdrachtWithNewAdres( model.AantalStudenten,
                        model.Schooljaar,
                        model.Semesters, model.Title,
                        model.Omschrijving,
                        model.Vaardigheden, model.Specialisatie, bedrijf,
                        model.Straat,(int)model.Nummer
                        , model.Gemeente, specialisatieRepository,
                        gemeenteRepository,statusRepository);
                    }
                    else
                    {
                        ViewBag.Error = "Nummer veld is verplicht";
                    }

                }
                b.AddOpdracht(opdracht);
                bedrijfRepository.SaveChanges();
                if (button.Equals("contact"))
                {
                    TempData["Info"] = "Opdracht " + opdracht.Title + " werd succesvol aangemaakt";
                    return RedirectToAction("AddContactToOpdracht", "Bedrijf",opdracht );
                }
                else
                {
                    TempData["Info"] = "Opdracht " + opdracht.Title + " werd succesvol aangemaakt";
                     return RedirectToAction("OpdrachtDetail","Bedrijf",opdracht); 
                }
               
            }

            return
                View(new CreateOpdrachtViewModel(specialisaties, b.ContactPersonen, new OpdrachtViewModel(),
                   id,gemeenteRepository));

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
           CreateContactPersoonView ccp = new CreateContactPersoonView(opdracht,b.ContactPersonen);
           return View(ccp);
       }
        
        // Contact wordt toegevoegd
        [HttpPost]
        public ActionResult AddContact(ContactModel contact, string id)
        {
            if (ModelState.IsValid)
            {
                FindBedrijf(id).AddContactPersoon(new ContactPersoon(contact.Naam, contact.Voornaam, contact.Functie, contact.ContactEmail, contact.ContactTelNr, contact.GsmNummer));
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
            CreateContactPersoonView ccpv = new CreateContactPersoonView(opdracht,b.ContactPersonen);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_UitLijst",ccpv);
            }
            return View("Index", b);
        }

        public ActionResult GetStageBegeleiderFromList(int id)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            Bedrijf bedrijf = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            CreateContactPersoonView ccpv = new CreateContactPersoonView(opdracht,bedrijf.ContactPersonen);
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
                return PartialView("_ContactFormPartial",model);
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
        public ActionResult MakeContactFromList([Bind(Prefix = "ContactToOpdrachtFromList")]AddContactToOpdrachtView model, int id)
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
                    return PartialView("_ContactDetail",c);
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

                ContactPersoon c = b.FindContactPersoon(model.ContactPersoon);
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
                    ContactEmail = model.ContactEmail,
                    ContactTelNr = model.ContactTelNr,
                    Functie = model.Functie,
                    GsmNummer = model.GsmNummer,
                    Naam = model.Naam,
                    Voornaam = model.Voornaam
                };
                bedrijf.AddContactPersoon(c);
                opdracht.Ondertekenaar = c;
                bedrijfRepository.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_ContactDetail", c);
                }
            }
            return View("_ContactFormPartial",model);
        }
    
        [HttpPost]
        public ActionResult MakeStageBegeleiderFromForm(ContactModel model, int id)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            Bedrijf bedrijf = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            if (ModelState.IsValid)
            {
                ContactPersoon c = new ContactPersoon()
                {
                    ContactEmail = model.ContactEmail,
                    ContactTelNr = model.ContactTelNr,
                    Functie = model.Functie,
                    GsmNummer = model.GsmNummer,
                    Naam = model.Naam,
                    Voornaam = model.Voornaam
                };
                bedrijf.AddContactPersoon(c);
                opdracht.StageMentor = c;
                bedrijfRepository.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_ContactDetail", c);
                }
            }
            return View("_StageBegeleiderForm",model);
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
        public ActionResult RemoveOpdracht(int id,String button)
        {
            Opdracht opdracht = opdrachtRepository.FindOpdracht(id);
            Bedrijf bedrijf = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            String title = opdracht.Title;
            if (button.Equals("ja"))
            {
                
                opdrachtRepository.RemoveOpdracht(opdracht);
                opdrachtRepository.SaveChanges();
                TempData["Info"] = "De opdracht "+title+" werd verwijderd";
                return RedirectToAction("Index","Bedrijf", bedrijf);
            }
            else
            {
                TempData["Message"] = "De opdracht " + title + " werd niet verwijderd";
                return RedirectToAction("Index","Bedrijf", bedrijf);
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
            try
            {
                if (button.Equals("ja"))
                {
                    contactPersoonRepository.VerwijderContact(contact);
                    TempData["Info"] = "De contactpersoon, " + title + ", werd verwijderd";
                    return RedirectToAction("BeheerContacten", b);
                }
                else
                {
                    TempData["Message"] = "De opdracht " + title + " werd niet verwijderd";
                    return RedirectToAction("BeheerContacten", b);
                }
            }
            catch (Exception e)
            {

                TempData["Message"] = e.Message;
                return View("ToRemoveContact", contact);
            }

        }
    }
}