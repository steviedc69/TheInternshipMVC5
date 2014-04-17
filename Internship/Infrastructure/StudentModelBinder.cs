using System;
using System.Web.Mvc;
using Internship.Models.Domain;
using Internship.Models.DAL;


namespace Internship.Infrastructure
{
    public class StudentModelBinder : IModelBinder
    {
        public object  BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
           
            if (controllerContext.HttpContext.User.Identity.IsAuthenticated)
            {
                IStudentRepository repos = (IStudentRepository)DependencyResolver.Current.GetService(typeof (IStudentRepository));
                return repos.FindByEmail(controllerContext.HttpContext.User.Identity.Name);
            }
           return null;
        }
    }
}
