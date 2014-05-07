using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Internship.Models.Domain;
using Microsoft.Ajax.Utilities;
using Ninject;


namespace Internship.ViewModels
{
    public class CreateOpdrachtViewModel
    {
        public String BedrijfId { get; set; }
        public ContactModel ContactModelOndertekenaar { get; set; }
        public ContactModel ContactModelStageMentor { get; set; }
        public OpdrachtViewModel OpdrachtViewModel { get; set; }
        public SelectList SpecialisatieList { get; private set; }
        public SelectList SemesterLijst { get; private set; }
        public SelectList SchooljaarSelectList { get; private set; }
        public SelectList OndertekenaarSelectList { get; private set; }
        public SelectList StageMentorSelectList { get; private set; }
        public SelectList AantalStudenten { get; private set; }
        public SelectList Gemeenten { get; private set; }
        public Opdracht Opdracht { get; set; }

        public CreateOpdrachtViewModel(IEnumerable<Specialisatie> specialisaties,
            IEnumerable<ContactPersoon> contactPersonen,
            OpdrachtViewModel opdrachtViewModel, String berdrijfId, IGemeenteRepository gemeenteRepository)
        {
            ContactModelOndertekenaar = new ContactPersoon().ConvertToContactCreateModel(BedrijfId);
            ContactModelStageMentor = new ContactPersoon().ConvertToContactCreateModel(BedrijfId);
            SpecialisatieList = new SelectList(specialisaties);
            List<String> lijstSemester = new List<string>(new String[] {"Semester 1", "Semester 2", "Semester 1 en 2"});
            SemesterLijst = new SelectList(lijstSemester);
            SchooljaarSelectList = new SelectList(MakeSchooljaarSelectList());
            OpdrachtViewModel = opdrachtViewModel;
            OndertekenaarSelectList = new SelectList(contactPersonen);
            StageMentorSelectList = new SelectList(contactPersonen);
            AantalStudenten = new SelectList(new int[] {1, 2, 3, 4, 5});
            BedrijfId = berdrijfId;
            IEnumerable<Gemeente> gem = gemeenteRepository.GetAlleGemeentes();
            Gemeenten = new SelectList(gem);
            //FillOpdrachtView();
        }

        public void FillOpdrachtView()
        {
            if (Opdracht !=null)
            {
                OpdrachtViewModel.AantalStudenten = Opdracht.AantalStudenten;
                OpdrachtViewModel.Gemeente = Opdracht.Adres.Gemeente.Structuur;
                OpdrachtViewModel.Omschrijving = Opdracht.Omschrijving;
                OpdrachtViewModel.Schooljaar = Opdracht.Schooljaar;
                OpdrachtViewModel.Specialisatie = Opdracht.Specialisatie.Title;
                OpdrachtViewModel.Straat = Opdracht.Adres.StraatNaam;
                OpdrachtViewModel.Nummer = Opdracht.Adres.Nummer;
                OpdrachtViewModel.Title = Opdracht.Title;
                OpdrachtViewModel.Vaardigheden = Opdracht.Vaardigheden;
                OpdrachtViewModel.Id = Opdracht.Id;
            }
        }

        private List<String> MakeSchooljaarSelectList()
        {
            List<String> lijstSchooljaren = new List<string>();
            DateTime date = DateTime.Now;
            int year = date.Year;
            if (date.Month < 2)
            {
                for (int i = -1; i < 5; i++)
                {
                    String schooljaar = (year + i) + " - " + (year + i + 1);
                    lijstSchooljaren.Add(schooljaar);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    
                    String schooljaar = (year + i) + " - " + (year + i + 1);
                    lijstSchooljaren.Add(schooljaar);
                }
            }

            return lijstSchooljaren;
        }
    }

    public class OpdrachtViewModel
    {

        public static int Id { get; set; }
        [Display(Name = "Titel : ")]
        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(50, ErrorMessage = "{0} is te lang.")]
        public String Title { get; set; }

        [Display(Name = "Opdracht omschrijving : ")]
        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(500, ErrorMessage = "{0} is te lang.")]
        public String Omschrijving { get; set; }

        [Required(ErrorMessage = "Er moet een keuze gemaakt worden.")]
        [Display(Name = "Semester : ")]
        public String Semesters { get; set; }

        [Display(Name = "Specialisatie : ")]
        [Required(ErrorMessage = "U moet een specialisatie kiezen.")]
        public String Specialisatie { get; set; }

        [Display(Name = "Schooljaar : ")]
        [Required(ErrorMessage = "U moet een schooljaar kiezen")]
        public String Schooljaar { get; set; }

        [Display(Name = "Kennis en vaardigheden : ")]
        [Required(ErrorMessage = "Geef een aantal basis vaardigheden op")]
        [StringLength(500, ErrorMessage = "{0} is te lang.")]
        public String Vaardigheden { get; set; }

        [Display(Name = "Aantal studenten :")]
        [Required(ErrorMessage = "Minimum 1 student aanduiden")]
        [Range(1, 10, ErrorMessage = "Minimum 1 student aanduiden")]
        public int AantalStudenten { get; set; }

        [Display(Name = "Zelfde adres als hoofdzetel : ")]
        public bool IsBedrijfAdres { get; set; }

        [Display(Name = "Straat : ")]
        public String Straat { get; set; }

        [Display(Name = "Nummer : ")]
        [Optional]
        [Range(1, 1999, ErrorMessage = "Nummer moet tussen 1 en 1999 liggen")]
        public int? Nummer { get; set; }

        [Display(Name = "Gemeente : ")]
        public String Gemeente { get; set; }

    }

    public class AddContactToOpdrachtView
    {
        [Display(Name = "Contact : ")]
        [Required (ErrorMessage = "Contactpersoon is verplicht")]
        public String ContactPersoon { get; set; }   
    }

    public class CreateContactPersoonView
    {
        public SelectList SoortPersoon { get; set; }
        public SelectList StageMentorSelectList { get; private set; }
        public AddContactToOpdrachtView ContactToOpdrachtFromList { get; set; }
        public ContactModel NewContactViewModel { get; set; }
        public Opdracht Opdracht { get; set; }

        public CreateContactPersoonView(Opdracht opdracht, IEnumerable<ContactPersoon> contactpersonen)
        {
            Opdracht = opdracht;
            List<String> soorten = new List<string>(new String[]{"Ondertekenaar v/h contract","Stagementor","Beiden"});
            if (opdracht.Ondertekenaar != null)
            {
                soorten.Remove("Ondertekenaar v/h contract");
                soorten.Remove("Beiden");
            }
            else if (opdracht.StageMentor != null)
            {
                soorten.Remove("Stagementor");
                soorten.Remove("Beiden");
            }
            SoortPersoon = new SelectList(soorten);
            StageMentorSelectList = new SelectList(contactpersonen);
            ContactToOpdrachtFromList = new AddContactToOpdrachtView();
            NewContactViewModel = new ContactModel();
            

        }


    
}



}


