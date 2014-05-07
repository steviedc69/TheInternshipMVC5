using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Internship.Models.Domain
{
    public class Status
    {
        public int Id { get; set; }
        public String Naam { get; set; }
        public String PanelClass { get; set; }
        public String AlertClass { get; set; }
    

    public Status()
        {
            
        }
    }
}