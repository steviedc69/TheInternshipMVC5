using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Internship.Models.Domain;


namespace Internship.ViewModels
{
    public class RegistratieModelCreater
    {
        public RegistratieModel RegistratieModel { get; set; }
        public SelectList GemeenteLijst { get; set; }

        public RegistratieModelCreater(IGemeenteRepository repository,RegistratieModel model)
        {
            IEnumerable<Gemeente> gemeentes = repository.GetAlleGemeentes();
            GemeenteLijst = new SelectList(gemeentes);
            RegistratieModel = model;
        }
    }

    public class RegistratieModel
    {

        [Display(Name="E-mailadres")]
        [Required(ErrorMessage = "{0} is verplicht")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "{0} is niet correct.")]
        public String Email { get; set; }
        [Display(Name = "Bedrijfsnaam")]
        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(50, ErrorMessage = "{0} is te lang.")]
        public String Bedrijfsnaam { get; set; }
        [Display(Name = "Website : ")]
        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(50, ErrorMessage = "{0} is te lang.")]
        [DataType(DataType.Url)]
        [RegularExpression(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$", ErrorMessage = "{0} is niet correct")]
        public String Url { get; set; }
        [StringLength(50, ErrorMessage = "{0} is te lang.")]
        public String Straat { get; set; }
        public int Straatnummer { get; set; }
        [Display(Name = "Gemeente : ")]
        public String Woonplaats { get; set; }
        [StringLength(10,ErrorMessage = "{0} is te lang")]
        [Required(ErrorMessage = "{0} is verplicht")]
        public String Telefoon { get; set; }
        public String Bereikbaarheid { get; set; }
        public String Activiteit { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Paswoorden moeten minstens 8 tekens lang zijn.")]
        [DataType(DataType.Password)]
        [Display(Name = "Paswoord")]
        public String Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Bevestig paswoord")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Paswoorden moeten gelijk zijn")]
        public string ConfirmPaswoord { get; set; }

    }
}