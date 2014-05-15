using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;

namespace Internship.Models.Domain
{
    public class Stagebegeleider : ApplicationUser
    {
    

        public String Naam { get; set; }
        public String Voornaam { get; set; }
        public String Gsmnummer { get; set; }
        [InverseProperty("StageBegeleiderPreference")]
        public virtual ICollection<Opdracht> Preferences { get; set; }

        [InverseProperty("Begeleider")]
        public virtual ICollection<Opdracht> TeBegeleidenOpdrachten { get; set; }

        public Boolean IsFirstTime { get; set; }

        private Sort Sort { get; set; }
        private ISearchStragegy SearchStragegy { get; set; }
        public Stagebegeleider()
        {
            IsFirstTime = true;
        }

        public void AddToPreferences(Opdracht opdracht)
        {
            Preferences.Add(opdracht);
        }

        public void RemoveFromPreferences(Opdracht opdracht)
        {
            Preferences.Remove(opdracht);
        }

        public IList<Opdracht> Sorteer()
        {
           return this.Sort.Sorteer();
        }

        public void AddSort(Sort sort)
        {
            this.Sort = sort;
        }

        public IList<Bedrijf> GetBedrijvenList()
        {
            IList<Bedrijf> list = new List<Bedrijf>();
            foreach (Opdracht opdracht in TeBegeleidenOpdrachten)
            {
                Bedrijf b = opdracht.Bedrijf;
                if (!list.Contains(b))
                {
                    list.Add(b);
                }
            }
            return list;
        }

        public IList<Student> GetStudentenList()
        {
            IList<Student>list = new List<Student>();
            foreach (Opdracht opdracht in TeBegeleidenOpdrachten)
            {
                IList<Student> s = opdracht.StageStudenten.ToList();
                foreach (Student student in s)
                {
                    list.Add(student);
                }
            }
            return list;
        }

        public void AddSearch(ISearchStragegy search)
        {
            this.SearchStragegy = search;
        }

        public IList<Opdracht> SearchResult(IList<Opdracht> opdrachten)
        {
            return SearchStragegy.Search(opdrachten, null);
        }
    }
}
