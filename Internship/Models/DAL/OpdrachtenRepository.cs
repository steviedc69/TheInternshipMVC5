using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Internship.Models.Domain;

namespace Internship.Models.DAL
{
    public class OpdrachtenRepository : IOpdrachtRepository
    {
        private InternshipContext Context;
        private DbSet<Opdracht> opdrachten; 

        public OpdrachtenRepository(InternshipContext context)
        {
            this.Context = context;
            this.opdrachten = context.Opdrachten;
        }


        public Opdracht FindOpdracht(int id)
        {
            return opdrachten.Find(id);
        }
    }
}