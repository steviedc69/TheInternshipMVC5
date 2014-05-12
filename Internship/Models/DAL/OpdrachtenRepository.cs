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
        private DbSet<Opdracht> Opdrachten; 

        public OpdrachtenRepository(InternshipContext context)
        {
            this.Context = context;
            this.Opdrachten = context.Opdrachten;
        }


        public Opdracht FindOpdracht(int id)
        {
            return Opdrachten.Find(id);
        }

       

        public IEnumerable<Opdracht> GeefStageOpdrachten()
        {
            return Opdrachten.Where(o=>o.Status.Naam.Equals("Stage")).OrderBy(o=>o.Schooljaar);
        }

        public IEnumerable<Opdracht> GeefStageOpdrachtenMetZoekstring(string search)
        {
            IEnumerable<Opdracht> opd = GeefStageOpdrachten();
            return opd.Where(o => o.Title.Contains(search)
                ||o.Omschrijving.Contains(search)||o.Vaardigheden.Contains(search)
                ||o.Specialisatie.Title.Contains(search)||o.Adres.Gemeente.Structuur.Contains(search)
                ||o.Bedrijf.Bedrijfsnaam.Contains(search));
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public void RemoveOpdracht(Opdracht opdracht)
        {
            Opdrachten.Remove(opdracht);

        }
    }
}