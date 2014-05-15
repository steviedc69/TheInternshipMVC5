using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
}