using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Internship.Models.Domain;
using Internship.ViewModels;
using Microsoft.Ajax.Utilities;

namespace Internship.Controllers
{
    [Authorize]
    public class BedrijfController : Controller
    {
        private IBedrijfRepository bedrijfRepository;
        private IStudentRepository studentRepository;
        private IStagebegeleiderRepository stagebegeleiderRepository;
        private IUserRepository userRepository;
        private ISpecialisatieRepository specialisatieRepository;
        private IOpdrachtRepository opdrachtRepository;

        //public UserController(){}

        public BedrijfController(IBedrijfRepository bedrijfR, IStudentRepository studentR,
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
        // GET: /Bedrijf/
        public ActionResult Index(String id)
        {
            Bedrijf b = bedrijfRepository.FindById(id);
            return View(b);
        }

        public ActionResult AddOpdracht(String id)
        {
            IEnumerable<Specialisatie> specialisaties;
            Bedrijf b = bedrijfRepository.FindById(id);
            specialisaties = specialisatieRepository.FindAllSpecialisaties();
            CreateOpdrachtViewModel model = new CreateOpdrachtViewModel(specialisaties, b.ContactPersonen,
                new OpdrachtViewModel(), id);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_CreateContact");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddOpdracht([Bind(Prefix = "OpdrachtViewModel")] OpdrachtViewModel model, String id)
        {
            IEnumerable<Specialisatie> specialisaties;
            Bedrijf b = bedrijfRepository.FindById(id);
            specialisaties = specialisatieRepository.FindAllSpecialisaties();
            if (ModelState.IsValid)
            {

                Bedrijf bedrijf = bedrijfRepository.FindById(id);
                /*Opdracht opdracht = DomainFactory.CreateOpdracht(model, bedrijf, specialisatieRepository);
                bedrijf.AddOpdracht(opdracht);
                bedrijfRepository.SaveChanges();
                 */
                CreateOpdrachtViewModel createOpdrachtViewModel = new CreateOpdrachtViewModel(specialisaties,bedrijf.ContactPersonen,model, id);
                return RedirectToAction("Index",createOpdrachtViewModel);
            }

            return
                View(new CreateOpdrachtViewModel(specialisaties, b.ContactPersonen, new OpdrachtViewModel(),
                   id));

        }

        public ActionResult BeheerContacten(String id)
        {
            Bedrijf bedrijf = bedrijfRepository.FindById(id);
            return View(bedrijf);

        }
      


}
}