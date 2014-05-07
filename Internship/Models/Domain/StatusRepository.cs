using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Internship.Models.DAL;

namespace Internship.Models.Domain
{
    public class StatusRepository : IStatusRepository
    {

        private DbSet<Status> statussen;
        private InternshipContext context;


        public StatusRepository(InternshipContext context)
        {
            this.context = context;
            statussen = context.Statussen;
        }

        public Status FindStatusWithId(int id)
        {
            return statussen.Find(id);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public Status FindStatusWithNaam(string naam)
        {
            return statussen.SingleOrDefault(s => s.Naam.Equals(naam));
        }
    }
}