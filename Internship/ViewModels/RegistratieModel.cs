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
        public UpdateModel UpdateModel { get; set; }
        public RegistratieModelStudent ModelStudent { get; set; }

        public RegistratieModelCreater(IGemeenteRepository repository,RegistratieModel model)
        {
            IEnumerable<Gemeente> gemeentes = repository.GetAlleGemeentes();
            GemeenteLijst = new SelectList(gemeentes);
            RegistratieModel = model;

        }

        public RegistratieModelCreater(IGemeenteRepository repository, UpdateModel umodel )
        {
            IEnumerable<Gemeente> gemeentes = repository.GetAlleGemeentes();
            GemeenteLijst = new SelectList(gemeentes);
            UpdateModel = umodel;
        }

        public RegistratieModelCreater(IGemeenteRepository repository, RegistratieModelStudent studentModel)
        {
            IEnumerable<Gemeente> gemeentes = repository.GetAlleGemeentes();
            GemeenteLijst = new SelectList(gemeentes);
            ModelStudent = studentModel;

        }
    }

    public class RegistratieModelStudent
    {
        [Display(Name = "Naam : ")]
        [Required(ErrorMessage = "{0} is verplicht")]
        public String Naam { get; set; }
        [Display(Name = "Voornaam : ")]
        [Required(ErrorMessage = "{0} is verplicht")]
        public String Voornaam { get; set; }
        [Display(Name = "GSM nummer : ")]
        [RegularExpression(@"^\(?(04)[1-9]{2}\)?(\-|\s)?[0-9]{6}$", ErrorMessage = "vb: 0494123456")]
        public String GsmNummer { get; set; }
        [Display(Name = "Straat : ")]
        public String Straat { get; set; }
        [Display(Name = "Nummer :")]
        [Range(1, 1999, ErrorMessage = "{0} moet verplicht een nummer zijn groter dan 0")]
        public int Straatnummer { get; set; }
        public String Gemeente { get; set; }
        
        
        [DataType(DataType.Date, ErrorMessage = "{0} moet een datum zijn")]
        [Display(Name = "Geboortedatum :")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",ApplyFormatInEditMode = true)]
      
        public DateTime? GeboorteDatum { get; set; }
        public String Image { get; set; }


    
}

    public class RegistratieModel
    {

        [Display(Name="E-mailadres*")]
        [Required(ErrorMessage = "{0} is verplicht")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "{0} is niet correct.")]
        public String Email { get; set; }
        [Display(Name = "Bedrijfsnaam*")]
        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(50, ErrorMessage = "{0} is te lang.")]
        public String Bedrijfsnaam { get; set; }
        [Display(Name = "Website* : ")]
        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(50, ErrorMessage = "{0} is te lang.")]
        [DataType(DataType.Url)]
        [RegularExpression(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$", ErrorMessage = "{0} is niet correct")]
        public String Url { get; set; }
        [StringLength(50, ErrorMessage = "{0} is te lang.")]
        [Required(ErrorMessage = "{0} is verplicht")]
        public String Straat { get; set; }
        [Range(1,1999,ErrorMessage = "{0} moet verplicht een nummer zijn groter dan 0")]
        public int Straatnummer { get; set; }
        [Display(Name = "Gemeente : ")]
        public String Woonplaats { get; set; }
        [StringLength(10,ErrorMessage = "{0} is te lang")]
        [Required(ErrorMessage = "{0} is verplicht")]
        public String Telefoon { get; set; }
        [Display(Name = "Openbaar vervoer :")]
        public Boolean Openbaarvervoer { get; set; }
        [Display(Name = "Per auto : ")]
        public Boolean PerAuto { get; set; }
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

    public class UpdateModel
    {

        [Display(Name = "Bedrijfsnaam* :")]
        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(50, ErrorMessage = "{0} is te lang.")]
        public String Bedrijfsnaam { get; set; }
        [Display(Name = "Website* : ")]
        [Required(ErrorMessage = "{0} is verplicht.")]
        [StringLength(50, ErrorMessage = "{0} is te lang.")]
        [DataType(DataType.Url)]
        [RegularExpression(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$", ErrorMessage = "{0} is niet correct")]
        public String Url { get; set; }
        [Display(Name = "Straat* :")]
        [StringLength(50, ErrorMessage = "{0} is te lang.")]
        [Required(ErrorMessage = "{0} is verplicht")]
        public String Straat { get; set; }
        [Display(Name = "Nummer* :")]
        [Required(ErrorMessage = "{0} is verplicht")]
        [Range(1, 1999, ErrorMessage = "{0} moet verplicht een nummer zijn groter dan 0")]
        public int Straatnummer { get; set; }
        [Display(Name = "Gemeente* : ")]
        public String Woonplaats { get; set; }
        [Display(Name = "Telefoon* :")]
        [StringLength(10, ErrorMessage = "{0} is te lang")]
        [Required(ErrorMessage = "{0} is verplicht")]
        public String Telefoon { get; set; }
        [Display(Name = "Openbaar vervoer :")]
        public Boolean Openbaarvervoer { get; set; }
        [Display(Name = "Per auto : ")]
        public Boolean PerAuto { get; set; }
        public String Activiteit { get; set; }
        [Display(Name = "Logo : ")]
        public String Image { get; set; }
    }
}