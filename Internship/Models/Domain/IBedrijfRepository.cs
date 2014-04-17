using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Internship.Models.Domain;

namespace Internship.Models.Domain
{
    public interface IBedrijfRepository
    {
        IQueryable<Bedrijf> FindAll();
        Bedrijf FindByEmail(string email);
        Bedrijf FindById(String id);
        Bedrijf FindByName(string bedrijfsnaam);
        IQueryable<Bedrijf> FindByPlace(Gemeente gemeente);
        void Add(Bedrijf bedrijf);

        void SaveChanges();

    }
}