using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Internship.Models.Domain;

namespace Internship.ViewModels
{
    public class StudentIndexModel
    {
        public Student Student { get; set; }
        public IEnumerable<Opdracht> Opdrachten { get; set; }
        public String Search { get; set; }

    public StudentIndexModel(IStudentRepository studentRepository,String id, IOpdrachtRepository opdrachtRepository,string search = null)
        {

            Student = createStudentFromRepo(id, studentRepository);
            if (search == null)
            {
                Search = "";
                Opdrachten = opdrachtRepository.GeefStageOpdrachten();
            }
            else
            {
                Search = search;
                Opdrachten = opdrachtRepository.GeefStageOpdrachtenMetZoekstring(search);
            }
            

        }

        private Student createStudentFromRepo(String id, IStudentRepository repository)
        {
            return repository.FindById(id);
        }
        
    }

    public class StudentOpdrachtDetailModel
    {
        public Bedrijf Bedrijf { get; set; }
        public Opdracht Opdracht { get; set; }
        public Student Student { get; set; }

        public StudentOpdrachtDetailModel(IStudentRepository studentRepository, String id,
            IOpdrachtRepository opdrachtRepository, int idOp, IBedrijfRepository bedrijfRepository)
        {
            Bedrijf = bedrijfRepository.FindBedrijfByOpdrachtId(idOp);
            Opdracht = opdrachtRepository.FindOpdracht(idOp);
            Student = studentRepository.FindById(id);
        }

        public StudentOpdrachtDetailModel(int id, IBedrijfRepository bedrijfRepository, IOpdrachtRepository opdrachtRepository)
        {
            Bedrijf = bedrijfRepository.FindBedrijfByOpdrachtId(id);
            Opdracht = opdrachtRepository.FindOpdracht(id);

        }
}
}
