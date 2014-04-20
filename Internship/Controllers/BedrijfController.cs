using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Internship.Models.Domain;
using Internship.ViewModels;
using Microsoft.Ajax.Utilities;

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

        //public UserController(){}

        public BedrijfController(IBedrijfRepository bedrijfR, IStudentRepository studentR,
            IStagebegeleiderRepository stagebegeleiderR, IUserRepository usersRepository,
            ISpecialisatieRepository specialisatie, IOpdrachtRepository opdracht)
        {
            this.bedrijfRepository = bedrijfR;
            this.stagebegeleiderRepository = stagebegeleiderR;
            this.studentRepository = studentR;
            this.userRepository = usersRepository;
            this.specialisatieRepository = specialisatie;
            this.opdrachtRepository = opdracht;
        }

        //
        // GET: /Bedrijf/
        public ActionResult Index(String id)
        {
            Bedrijf b = bedrijfRepository.FindById(id);
            return View(b);
        }

        public ActionResult AddOpdracht(String id = null, ContactModel modelContact = null)
        {
            IEnumerable<Specialisatie> specialisaties;
            Bedrijf b = bedrijfRepository.FindById(id);
            specialisaties = specialisatieRepository.FindAllSpecialisaties();
            CreateOpdrachtViewModel model = new CreateOpdrachtViewModel(specialisaties, b.ContactPersonen,
                new OpdrachtViewModel(), id);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_CreateContact", modelContact);
            }

            return View(model);
        }



        [HttpPost]
        public ActionResult AddOpdracht([Bind(Prefix = "CreateOpdrachtViewModel")] CreateOpdrachtViewModel model, String id)
        {
            IEnumerable<Specialisatie> specialisaties;
            Bedrijf b = bedrijfRepository.FindById(id);
            specialisaties = specialisatieRepository.FindAllSpecialisaties();
            if (ModelState.IsValid)
            {

                Bedrijf bedrijf = bedrijfRepository.FindById(id);
                Opdracht opdracht = new Opdracht();
                ContactPersoon contactOndertekenaar = new ContactPersoon();
                if (model.OndertekenaarSelectList.SelectedValue == null)
                {

                    contactOndertekenaar.ContactEmail = model.ContactModelOndertekenaar.ContactEmail;
                    contactOndertekenaar.ContactTelNr = model.ContactModelOndertekenaar.ContactTelNr;
                    contactOndertekenaar.Functie = model.ContactModelOndertekenaar.Functie;
                    contactOndertekenaar.GsmNummer = model.ContactModelOndertekenaar.GsmNummer;
                    contactOndertekenaar.Naam = model.ContactModelOndertekenaar.Naam;
                    contactOndertekenaar.Voornaam = model.ContactModelOndertekenaar.Voornaam;
                    bedrijf.AddContactPersoon(contactOndertekenaar);
                    opdracht.Ondertekenaar = contactOndertekenaar;
                }
                else
                {
                    opdracht.Ondertekenaar = bedrijf.FindContactPersoon(model.OndertekenaarSelectList.SelectedValue.ToString());
                }

                ContactPersoon contactStageMentor = new ContactPersoon();
                if (model.OndertekenaarSelectList.SelectedValue == null)
                {

                    contactStageMentor.ContactEmail = model.ContactModelOndertekenaar.ContactEmail;
                    contactStageMentor.ContactTelNr = model.ContactModelOndertekenaar.ContactTelNr;
                    contactStageMentor.Functie = model.ContactModelOndertekenaar.Functie;
                    contactStageMentor.GsmNummer = model.ContactModelOndertekenaar.GsmNummer;
                    contactStageMentor.Naam = model.ContactModelOndertekenaar.Naam;
                    contactStageMentor.Voornaam = model.ContactModelOndertekenaar.Voornaam;
                    bedrijf.AddContactPersoon(contactStageMentor);
                    opdracht.StageMentor = contactStageMentor;
                }
                else
                {
                    opdracht.StageMentor = bedrijf.FindContactPersoon(model.StageMentorSelectList.SelectedValue.ToString());
                }

                if (model.SemesterLijst.SelectedValue.Equals("Semester 1"))
                {
                    opdracht.IsSemester1 = true;
                    opdracht.IsSemester2 = false;
                }
                else if (model.SemesterLijst.SelectedValue.Equals("Semester 2"))
                {
                    opdracht.IsSemester1 = false;
                    opdracht.IsSemester2 = true;
                }
                else
                {
                    opdracht.IsSemester1 = true;
                    opdracht.IsSemester2 = true;
                }

                opdracht.Omschrijving = model.OpdrachtViewModel.Omschrijving;
                opdracht.Schooljaar = model.SchooljaarSelectList.SelectedValue.ToString();
                opdracht.Specialisatie =
                    specialisatieRepository.FindSpecialisatieNaam(model.SpecialisatieList.SelectedValue.ToString());
                opdracht.Title = model.OpdrachtViewModel.Title;
                opdracht.Vaardigheden = model.OpdrachtViewModel.Vaardigheden;
                opdracht.AantalStudenten = (int)model.AantalStudenten.SelectedValue;
                
                opdracht.ActivatieDatum = DateTime.Now;
                
                opdracht.Adres = bedrijf.Adres;

                /*Opdracht opdracht = DomainFactory.CreateOpdracht(model, bedrijf, specialisatieRepository);
                bedrijf.AddOpdracht(opdracht);
                bedrijfRepository.SaveChanges();
                 */
                CreateOpdrachtViewModel createOpdrachtViewModel = new CreateOpdrachtViewModel(specialisaties, bedrijf.ContactPersonen, model, id);
                return RedirectToAction("Index", createOpdrachtViewModel);
            }

            return
                View(new CreateOpdrachtViewModel(specialisaties, b.ContactPersonen, new OpdrachtViewModel(),
                   id));

        }

        public ActionResult BeheerContacten(String id)
        {
            Bedrijf bedrijf = bedrijfRepository.FindById(id);
            return View(bedrijf);

        }

        public ActionResult AddContact(String id)
        {
            return View(new ContactPersoon().ConvertToContactCreateModel(id));
        }
        // Contact wordt toegevoegd
        [HttpPost]
        public ActionResult AddContact(ContactModel contact, string id)
        {
            if (ModelState.IsValid)
            {
                bedrijfRepository.FindById(id).AddContactPersoon(new ContactPersoon(contact.Naam, contact.Voornaam, contact.Functie, contact.ContactEmail, contact.ContactTelNr, contact.GsmNummer));
                bedrijfRepository.SaveChanges();
                return View("Index");

            }
            return View(contact);
        }

    }
}