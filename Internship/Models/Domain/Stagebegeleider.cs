using System;
using System.Security.Policy;

namespace Internship.Models.Domain
{
    public class Stagebegeleider : ApplicationUser
    {
        private String naam;
        private String voornaam;
        private String gmsnummer;

        public String Naam { get; set; }
        public String Voornaam { get; set; }
        public String Gsmnummer { get; set; }
       
        public Stagebegeleider()
        {
            
        }

      
    }
}
