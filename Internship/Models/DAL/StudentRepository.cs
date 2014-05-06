using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Internship.Models.DAL;
using Internship.Models.Domain;

namespace Internship.Models.DAL
{
    public class StudentRepository : IStudentRepository
    {
        // Context en DbSet
        private InternshipContext context;
        private DbSet<Student> studenten;

        // Constructor
        public StudentRepository(InternshipContext context)
        {
            this.context = context;
            studenten = context.Studenten;
        }

        public void AddStudent(Student student)
        {
            context.Studenten.Add(student);
            //context.SaveChanges();
        }

        public IQueryable<Student> FindAll()
        {
            return studenten.OrderBy(s => s.Naam);
        }

        public Student FindByEmail(string email)
        {
            return studenten.SingleOrDefault(s => s.UserName.Equals(email));
        }

        public Student FindById(string id)
        {
            return studenten.Find(id);
        }


        public IQueryable<Student> FindByName(string naam)
        {
            return studenten.Where(s => s.Naam == naam || s.Voornaam == naam);
        }

        public IQueryable<Student> FindByPlace(string gemeente)
        {
            return studenten.Where(s => s.Woonplaats == gemeente);
        }

        public void UpdateFirstTime(String username,bool notFirstTime)
        {
            Student s = FindByEmail(username);
            s.NotFirstTime = notFirstTime;
            studenten.AddOrUpdate(s);
            SaveChanges();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}