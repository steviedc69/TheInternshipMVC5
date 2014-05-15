using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace Internship.Models.Domain
{
    public class Student : ApplicationUser
    {


        public String Naam { get; set; }
        public String Voornaam { get; set; }
        public virtual Adres Adres { get; set; }
        public String Gsmnummer { get; set; }
        public String Gebdatum { get; set; }
        public bool NotFirstTime { get; set; }
        
        [InverseProperty("StudentenFavorites")]
        public virtual ICollection<Opdracht> Favorites { get; set; }
        [InverseProperty("StageStudenten")]
        public virtual Opdracht StageOpdracht { get; set; }
        public String ImageUrl { get; set; }

        public Student()
        {
            NotFirstTime = false;
            ImageUrl = "/Images/user.png";
        }

        public void AddOpdrachtToFavorites(Opdracht opdracht)
        {
            Favorites.Add(opdracht);

        }

        public void RemoveOprachtFromFavorites(Opdracht opdracht)
        {
            Favorites.Remove(opdracht);
        }

  
        
    }
}
