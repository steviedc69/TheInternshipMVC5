using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Internship.Models.Domain;

namespace Internship.Controllers
{
    public class StudentController : Controller
    {
        //
        // GET: /Student/
        public ActionResult Index(Student student)
        {
            return View();
        }
	}
}