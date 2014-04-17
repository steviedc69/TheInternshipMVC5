using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Internship.Models.Domain
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
            
        }

        public ApplicationRole(String naam, string description) : base(naam)
        {
            this.Description = description;
        }
        public virtual string Description { get; set; }

    }
}