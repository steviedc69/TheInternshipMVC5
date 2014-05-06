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

       

        public IEnumerable<Opdracht> GeefStageOpdrachten()
        {
            return opdrachten.Where(o=>o.IsStageOpdracht==true).OrderBy(o=>o.Schooljaar);
        }

        public IEnumerable<Opdracht> GeefStageOpdrachtenMetZoekstring(string search)
        {
            IEnumerable<Opdracht> opd = GeefStageOpdrachten();
            return opd.Where(o => o.Title.Contains(search)
                ||o.Omschrijving.Contains(search)||o.Vaardigheden.Contains(search)
                ||o.Specialisatie.Title.Contains(search));
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}