using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Internship.Models.Domain;
using Internship.ViewModels;
using Microsoft.AspNet.Identity;

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
        public ActionResult Index(String id, string search = null)
        {
            StudentIndexModel model = new StudentIndexModel(studentRepository, id, opdrachtRepository, search);
            if (Request.IsAjaxRequest())
            {
                model = new StudentIndexModel(studentRepository, id, opdrachtRepository, search);
                return PartialView("_StagesPart", model);
            }
            return View(model);
        }

        public ActionResult GetOpdrachtDetail(int? id,String student)
        {
            if (id.HasValue)
            {
                StudentOpdrachtDetailModel model = new StudentOpdrachtDetailModel(studentRepository, student, opdrachtRepository,(int)id, bedrijfRepository);
                return View("OpdrachDetail", model);
            }

            return RedirectToAction("Index", student);

        }

        [HttpPost]
        public ActionResult AddToFavorites(String id, int opdrId)
        {
            Student student = studentRepository.FindById(id);
            Opdracht opr = opdrachtRepository.FindOpdracht(opdrId);
            return View("Index");
        }
}
}