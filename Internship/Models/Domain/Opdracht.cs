using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Internship.Models.Domain
{
    public class Opdracht
    {
        /*
         * Een stage-opdracht bevat minstens volgende gegevens: titel, omschrijving opdracht,
         * specialisatie stageopdracht (programmeren, webontwikkeling, mainframe, e-business, mobile, systeembeheer..), 
         * sem 1 of sem2, aantal studenten opdracht, stagementor. De stage coördinator en de administratie 
         * studentensecretariaat dient een e-mail te ontvangen met de gepaste tekst over de (wijzigingen) stage-opdracht.
         */
        public int Id { get; set; }
        public String Title { get; set; }
        public String Omschrijving { get; set; }
        public String Vaardigheden { get; set; }
        public Boolean IsSemester1 { get; set; }
        public Boolean IsSemester2 { get; set; }
        public String Schooljaar { get; set; }
        //public virtual Bedrijf Bedrijf { get; set; }
        public virtual Specialisatie Specialisatie { get; set; }
        public String AdminComment { get; set; }
        public virtual ContactPersoon Ondertekenaar { get; set; }
        public virtual ContactPersoon StageMentor { get; set; }
        public DateTime ActivatieDatum { get; set; }
        public int AantalStudenten { get; set; }
        public virtual Adres Adres { get; set; }
        public bool IsStageOpdracht { get; set; }
        

         public Opdracht()
        {
            ActivatieDatum = DateTime.Now;
            IsStageOpdracht = false;
        }

      
    }
}