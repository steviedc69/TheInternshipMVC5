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


namespace Internship.ViewModels
{
    public class CreateOpdrachtViewModel
    {
        public static String BedrijfId { get; set; }
        public ContactModel ContactModelOndertekenaar { get; set; }
        public ContactModel ContactModelStageMentor { get; set; }
        public OpdrachtViewModel OpdrachtViewModel { get; set; }
        public SelectList SpecialisatieList { get; private set; }
        public SelectList SemesterLijst { get; private set; }
        public SelectList SchooljaarSelectList { get; private set; }
        public SelectList OndertekenaarSelectList { get; private set; }
        public SelectList StageMentorSelectList { get; private set; }
        public SelectList AantalStudenten { get; private set; }
        

    public CreateOpdrachtViewModel(IEnumerable<Specialisatie> specialisaties,IEnumerable<ContactPersoon>contactPersonen ,
        OpdrachtViewModel opdrachtViewModel,String berdrijfId)
    {
        ContactModelOndertekenaar = new ContactPersoon().ConvertToContactCreateModel(BedrijfId);
        ContactModelStageMentor = new ContactPersoon().ConvertToContactCreateModel(BedrijfId);
            SpecialisatieList = new SelectList(specialisaties);
            List<String> lijstSemester = new List<string>(new String[] {"Semester 1", "Semester 2", "Semester 1 en 2"});
            SemesterLijst = new SelectList(lijstSemester);
            SchooljaarSelectList = new SelectList(MakeSchooljaarSelectList());
            OpdrachtViewModel = OpdrachtViewModel;
            OndertekenaarSelectList = new SelectList(contactPersonen);
            StageMentorSelectList = new SelectList(contactPersonen);
            AantalStudenten = new SelectList(new int[]{1,2,3,4,5});
            BedrijfId = berdrijfId;
         
        }

       
        private List<String> MakeSchooljaarSelectList()
        {
            List<String> lijstSchooljaren = new List<string>();
            DateTime date = DateTime.Now;
            for (int i = 0; i < 5; i++)
            {
                int year = date.Year;
                String schooljaar = (year + i) + " - " + (year + i + 1);
                lijstSchooljaren.Add(schooljaar);
            }
            return lijstSchooljaren;
        }
    }

    public class OpdrachtViewModel
    {


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
        [Required(ErrorMessage = "Minimum 1 studenten aanduiden")]
        public int AantalStudenten { get; set; }

        [Display(Name = "Stage contract ondertekenaar : ")]
        [Required(ErrorMessage = "Een contactpersoon is hier verplicht")]
        public String ContractOndertekenaar { get; set; }
        public bool UitLijst { get; set; }
        [Display(Name = "Stage Mentor : ")]
        [Required(ErrorMessage = "Een contactpersoon is hier verplicht")]
        public String StageMentor { get; set; }
        public bool MentorUitLijst { get; set; }


        }

  

    }


