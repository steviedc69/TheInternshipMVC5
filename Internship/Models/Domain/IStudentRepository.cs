using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship.Models.Domain
{
    public interface IStudentRepository
    {
        
        void AddStudent(Student student);
        IQueryable<Student> FindAll();
        Student FindByEmail(string email);
        Student FindById(String id);
        IQueryable<Student> FindByName(string naam);
        IQueryable<Student> FindByPlace(string gemeente);
        void UpdateFirstTime(String username, bool notFirstTime);
        void SaveChanges();
    }
}
