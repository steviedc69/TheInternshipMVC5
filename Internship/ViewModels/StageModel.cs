using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Internship.Models.Domain;


namespace Internship.ViewModels
{
    public class StageModel
    {
        [DisplayName("Naam voor de stage : ")]
        [Required(ErrorMessage = "Naam van de stage moet ingevuld worden.")]
        public String Title { get; set; }
        [Required(ErrorMessage = "{0} moet ingevuld worden.")]
        [DisplayName("Omschrijving : ")]
        public String Omschrijving { get; set; }
        [DisplayName("Semester 1")]
        public Boolean IsSemester1 { get; set; }
        [DisplayName("Semester 2")]
        public Boolean IsSemester2 { get; set; }
        //public virtual Bedrijf Bedrijf { get; set; }
        [DisplayName("Specialisatie van de stage :")]
        public virtual Specialisatie Specialisatie { get; set; }
        public String AdminComment { get; set; }
    }
}