using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Internship.Models.Domain;

namespace Internship.Models.Domain
{
    public interface ISpecialisatieRepository
    {
        IQueryable<Specialisatie> FindAllSpecialisaties();
        Specialisatie FindSpecialisatie(int id);
        Specialisatie FindSpecialisatieNaam(String naam);
        void SaveChanges();
    }
}