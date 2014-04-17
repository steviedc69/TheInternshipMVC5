using System;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web.Hosting;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Internship.Models.Domain
{
    //kunnen geen User objecten rechtstreeks in de db, ofwel bedrijf, student, stagebegeleider
    public abstract class ApplicationUser : IdentityUser
    {


        protected ApplicationUser()
        {

        }

        //public String Emailadres{ get; set; }


    }


}

