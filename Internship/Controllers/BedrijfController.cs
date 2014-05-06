using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
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

        //public UserController(){}

        public BedrijfController(IBedrijfRepository bedrijfR, IStudentRepository studentR,
            IStagebegeleiderRepository stagebegeleiderR, IUserRepository usersRepository,
            ISpecialisatieRepository specialisatie, IOpdrachtRepository opdracht,IGemeenteRepository gemeenteRepository)
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
                new OpdrachtViewModel(){Straat = b.Adres.StraatNaam,Nummer = b.Adres.Nummer,Gemeente = b.Adres.Gemeente.Structuur}, id,gemeenteRepository);
            return View(model);
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
                        specialisatieRepository,gemeenteRepository);
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
                        gemeenteRepository);
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
                    return RedirectToAction("AddContactToOpdracht", "Bedrijf",opdracht );
                }
                else
                {
                     return RedirectToAction("Index","Bedrijf",bedrijf); 
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
    }
}