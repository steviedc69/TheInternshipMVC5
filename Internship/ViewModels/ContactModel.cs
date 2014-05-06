using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web;

namespace Internship.ViewModels
{
    public class ContactModel
    {
        public String BedrijfsId { get; set; }
        public int id { get; set; }
        [Required(ErrorMessage = "{0} is verplicht")]
        [Display(Name = "Naam : ")]
        public String Naam { get; set; }
        [Required(ErrorMessage = "{0} is verplicht")]
        [Display(Name = "Voornaam : ")]
        public String Voornaam { get; set; }
        [Required(ErrorMessage = "{0} is verplicht")]
        [Display(Name = "Functie binnen stagebedrijf :")]
        public String Functie { get; set; }
        [Required(ErrorMessage = "{0} is verplicht")]
        [Display(Name = "E-mailadres van contactpersoon : ")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "{0} is niet correct ingevoerd.")]
        public String ContactEmail { get; set; }
        [Required(ErrorMessage = "{0} is verplicht")]
        [Display(Name = "Telefoonnummer van contactpersoon : ")]
        [RegularExpression(@"^0\d{8}$",ErrorMessage = "vb: 011090909")]
        public String ContactTelNr { get; set; }
        [Display(Name = "GSM nummer van contactpersoon : ")]
        [RegularExpression(@"^\(?(04)[1-9]{2}\)?(\-|\s)?[0-9]{6}$", ErrorMessage = "vb: 0494123456")]
        public String GsmNummer { get; set; }
    }
    public class ContactDeleteViewModel
    {
        public int Id { get; set; }
        public String Naam { get; set; }
        public String Voornaam { get; set; }

        public ContactDeleteViewModel() { }

        public ContactDeleteViewModel(String naam, String voornaam, int id)
        {
            Id = id;
            Naam = naam;
            Voornaam = voornaam;
        }
    }
}