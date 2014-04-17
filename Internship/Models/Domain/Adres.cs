using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Internship.Models.Domain
{
    public class Adres
    {
        public int Id { get; set; }
        public String StraatNaam { get; set; }
        public int Nummer { get; set; }
        public Gemeente Gemeente { get; set; }

        public Adres()
        {
            
        }
    }
}