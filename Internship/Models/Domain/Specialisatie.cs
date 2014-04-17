using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Internship.Models.Domain
{
    public class Specialisatie
    {
        public int Id { get; set; }
        public String Title { get; set; }
        //public IList<Opdracht>Opdrachten { get; set; }

        public Specialisatie()
        {
          //Opdrachten = new List<Opdracht>();   
        }

        public Specialisatie(int id, String title)
        {
            this.Id = id;
            this.Title = title;
           // this.Opdrachten = opdrachten;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}