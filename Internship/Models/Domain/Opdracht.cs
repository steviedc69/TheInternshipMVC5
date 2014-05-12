using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
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

        private Boolean isSemester1;
        private Boolean isSemester2;


        public int Id { get; set; }
        public String Title { get; set; }
        public String Omschrijving { get; set; }
        public String Vaardigheden { get; set; }

        public String Schooljaar { get; set; }
        //public virtual Bedrijf Bedrijf { get; set; }
        public Boolean IsSemester1
        {
            get { return isSemester1; }
            set
            {
                if (!FirstSemesterPossible())
                {
                    throw new ArgumentException("1ste semester is niet meer mogelijk voor dit jaar");
                }
                isSemester1 = value;
            }
        }

        public Boolean IsSemester2
        {
            get { return isSemester2; }
            set
            {
                if (!SecondSemesterPossible())
                {
                    throw new ArgumentException("2de semester is niet meer mogelijk voor dit jaar");
                }
                isSemester2 = value;
            }
        }

        public virtual Specialisatie Specialisatie { get; set; }
        public String AdminComment { get; set; }
        public virtual ContactPersoon Ondertekenaar { get; set; }
        public virtual ContactPersoon StageMentor { get; set; }
        public DateTime ActivatieDatum { get; set; }
        public int AantalStudenten { get; set; }
        public virtual Adres Adres { get; set; }
        public virtual Status Status { get; set; }
        [InverseProperty("Favorites")]
        public virtual ICollection<Student> StudentenFavorites { get; set; }
        [InverseProperty("Preferences")]
        public virtual ICollection<Stagebegeleider> StageBegeleiderPreference { get; set; }
        [InverseProperty("Opdrachten")]
        public virtual Bedrijf Bedrijf { get; set; }
        public virtual ICollection<Student> StageStudenten { get; set; }
        public virtual Stagebegeleider Begeleider { get; set; }

        public Opdracht()
        {
            ActivatieDatum = DateTime.Now;


        }

        private Boolean FirstSemesterPossible()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            String[] schooljaar = Schooljaar.Split(' ');
            if (schooljaar[0].Equals("" + year))
            {
                if (month > 9)
                {
                    return false;
                }
                return true;
            }
            return true;
        }

        private Boolean SecondSemesterPossible()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            String[] schooljaar = Schooljaar.Split(' ');
            if (schooljaar[schooljaar.Length - 1].Equals("" + year))
            {
                if (month > 1)
                {
                    return false;
                }
                return true;
            }
            return true;
        }

        public ContactPersoon HasContact(ContactPersoon persoon)
        {
            if (Ondertekenaar != null)
            {


                if (Ondertekenaar.Id == persoon.Id)
                {
                    return persoon;
                }
            }
            if (StageMentor != null)
            {


                if (StageMentor.Id == persoon.Id)
                {
                    return persoon;
                }
            }
            return null;
        }

        public Boolean IsContactFromOpdracht(ContactPersoon persoon)
        {
            return HasContact(persoon) != null;
        }

        public String[] SplitSchooljaar()
        {
            String[] split = Regex.Split(Schooljaar, " - ");
            return split;
        }

        public String SchooljaarFirst()
        {
            return SplitSchooljaar()[0];
        }

        public String SchooljaarSecond()
        {
            return SplitSchooljaar()[1];
        }

        public int? SchoolJaarSecondInt()
        {
            return Bewerkingen.TryParseString(SchooljaarSecond());
        }
        public int? SchoolJaarFirstInt()
        {
            return Bewerkingen.TryParseString(SchooljaarFirst());
        }

        public override string ToString()
        {
          return ""+Title+Omschrijving+Schooljaar+Specialisatie.Title+Vaardigheden;
        }
    }
    

}
