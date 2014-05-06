using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace Internship.Models.Domain
{
    public class Student : ApplicationUser
    {


        public String Naam { get; set; }
        public String Voornaam { get; set; }
        public String Straat { get; set; }
        public int Straatnummer { get; set; }
        public String Woonplaats { get; set; }
        public int Postcode { get; set; }
        public String Gsmnummer { get; set; }
        public String Gebdatum { get; set; }
        public bool NotFirstTime { get; set; }
        public List<Opdracht> Favorites { get; set; } 


        public Student()
        {
            NotFirstTime = false;
            Favorites = new List<Opdracht>();
        }

        public void addOpdrachtToFavorites(Opdracht opdracht)
        {
            Favorites.Add(opdracht);

        }

        public void removeOprachtFromFavorites(Opdracht opdracht)
        {
            Favorites.Remove(opdracht);
        }

       
        
    }
}
