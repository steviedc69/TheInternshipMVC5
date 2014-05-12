using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Internship.Models.Domain;
using Internship.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Internship.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {

        private IBedrijfRepository bedrijfRepository;
        private IStudentRepository studentRepository;
        private IStagebegeleiderRepository stagebegeleiderRepository;
        private IUserRepository userRepository;
        private ISpecialisatieRepository specialisatieRepository;
        private IOpdrachtRepository opdrachtRepository;

        public StudentController(IBedrijfRepository bedrijfR, IStudentRepository studentR,
            IStagebegeleiderRepository stagebegeleiderR, IUserRepository usersRepository,
            ISpecialisatieRepository specialisatie, IOpdrachtRepository opdracht)
        {
            this.bedrijfRepository = bedrijfR;
            this.stagebegeleiderRepository = stagebegeleiderR;
            this.studentRepository = studentR;
            this.userRepository = usersRepository;
            this.specialisatieRepository = specialisatie;
            this.opdrachtRepository = opdracht;
        }

        //
        // GET: /Student/
        public ActionResult Index(String id,int?page,string search = null)
        {
            Student student = FindStudent(id);
            IList<Opdracht> opdrachten = opdrachtRepository.GeefStageOpdrachten().ToList();
            if (Request.IsAjaxRequest())
            {
                if (search != null)
                {
                    ViewBag.Selection = "Resultaten voor: " + search;
                    opdrachten = opdrachtRepository.GeefStageOpdrachtenMetZoekstring(search).ToList();
                }   
                return PartialView("_StagesPart", opdrachten.ToPagedList(page??1,12));
            }

            return View(opdrachten.ToPagedList(page??1,12));
        }

        public ActionResult GetOpdrachtDetail(int id)
        {
           
            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult AddToFavorites(int id)
        {
            //Student student = FindStudent(id);
            Opdracht opr = opdrachtRepository.FindOpdracht(id);
            Student student = FindStudent(User.Identity.GetUserId());
            student.AddOpdrachtToFavorites(opr);
            studentRepository.SaveChanges();

            if (Request.IsAjaxRequest())
            {
                return Content(opr.Title+" werd toegevoegd aan favorieten");
            }
            ViewBag["info"] = opr.Title + " werd toegevoegd aan favorieten";
            return RedirectToAction("Index", student);
        }

        public ActionResult GetFavorites(String id,int?page)
        {
            Student student = FindStudent(id);
            return View("Favoites",student.Favorites.ToList().ToPagedList(page?? 1, 12));
        }

        public Student FindStudent(String id)
        {
            return studentRepository.FindById(id);
        }
}
}