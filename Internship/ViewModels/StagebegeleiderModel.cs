using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Internship.Models.Domain;

namespace Internship.ViewModels
{
    public class StagebegeleiderModel
    {
        [Display(Name = "Naam")]
        [Required(ErrorMessage = "{0} is verplicht")]
        public String Naam { get; set; }
        [Display(Name = "Voornaam")]
        [Required(ErrorMessage = "{0} is verplicht")]
        public String Voornaam { get; set; }
        [Display(Name = "Gsm ")]
        public String GsmNummer { get; set; }

    }

    public class CreateSearchModel
    {
        public SelectList Bedrijven { get; set; }
        public SelectList Schooljaar { get; set; }
        public SearchModel Model { get; set; }
        public CreateSearchModel(IBedrijfRepository bedrijfRepository,SearchModel model)
        {
            List<String> jaren = Bewerkingen.MakeSchooljaarSelectList();
            jaren.Insert(0,"");
            Schooljaar = new SelectList(jaren);
            Bedrijven = new SelectList(bedrijfRepository.FindAll());
            Model = model;
        }

    }

    public class SearchModel
    {
       
        public String Bedrijven { get; set; }
        public String Schooljaar { get; set; }
        public String Gemeente { get; set; }
        public String Trefwoord { get; set; }
        
    }
}