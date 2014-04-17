using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Internship.Models.DAL;
using Internship.Models.Domain;
using Internship.Infrastructure;

namespace Internship
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            ModelBinders.Binders.Add(typeof(Bedrijf), new BedrijfModelBinder());
            ModelBinders.Binders.Add(typeof(Stagebegeleider), new StageModelBinder());
            ModelBinders.Binders.Add(typeof(Student), new StudentModelBinder());
            Database.SetInitializer<InternshipContext>(new InternshipInitializer());
            new InternshipContext().Bedrijven.ToList();
            
        }
    }
}
