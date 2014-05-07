using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Internship.Models.Domain;

using WebGrease.Css.Extensions;

namespace Internship.Models.DAL
{
    public class BedrijfRepository : IBedrijfRepository
    {
        // Context en DbSet
        private InternshipContext context;
        private DbSet<Bedrijf> bedrijven;
        private DbSet<Opdracht> opdrachten;
        private DbSet<ContactPersoon> contactpersonen;

        // Constructor
        public BedrijfRepository(InternshipContext context)
        {
            this.context = context;
            bedrijven = context.Bedrijven;
            opdrachten = context.Opdrachten;
            contactpersonen = context.ContactPersonen;
        }

        public IQueryable<Bedrijf> FindAll()
        {
            return bedrijven.OrderBy(b => b.Bedrijfsnaam);
        }

        public Bedrijf FindByEmail(string email)
        {
            return bedrijven.SingleOrDefault(b => b.UserName.Equals(email));
        }

        public Bedrijf FindById(String id)
        {
            return bedrijven.SingleOrDefault(b=>b.Id.Equals(id));
        }


        public Bedrijf FindByName(string bedrijfsnaam)
        {
            return bedrijven.FirstOrDefault(b => b.Bedrijfsnaam == bedrijfsnaam);
        }

        public IQueryable<Bedrijf> FindByPlace(Gemeente gemeente)
        {
            return bedrijven.Where(b => b.Adres.Gemeente == gemeente);
        }

        public void Add(Bedrijf bedrijf)
        {
            bedrijven.Add(bedrijf);
        }

        public Bedrijf FindBedrijfByOpdrachtId(int id)
        {
            List<Bedrijf> bedrijfs = FindAll().ToList();
            Bedrijf b = null;
            foreach (Bedrijf bedrijf in bedrijfs)
            {
                if (bedrijf.FindOpdracht(id)!=null)
                {
                    b = bedrijf;
                }
            }
            return b;
        }

        public Bedrijf FindBedrijfByContactPersId(int id)
        {
            List<Bedrijf> bedrijfs = FindAll().ToList();
            Bedrijf b = null;
            foreach (Bedrijf bedrijf in bedrijfs)
            {
                if (bedrijf.FindContactPersoonById(id) != null)
                {
                    b = bedrijf;
                }
            }
            return b;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

    }
}