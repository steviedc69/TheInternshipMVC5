using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Internship.Models.Domain
{
    public class Gemeente
    {
        public int Id { get; set; }
        public String Postcode { get; set; }
        public String Naam { get; set; }
        public String Up { get; set; }
        public String Structuur { get; set; }

        public Gemeente()
        {
            
        }

        public override string ToString()
        {
            return Structuur;
        }
    }
}