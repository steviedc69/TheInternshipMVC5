using System;
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
       
        public Student()
        {
            NotFirstTime = false;
        }

       
        
    }
}
