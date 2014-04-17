using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Internship.Models.Domain;

namespace Internship.Models.DAL
{
    public class GemeenteRepository : IGemeenteRepository
    {
        private InternshipContext context;
        private DbSet<Gemeente> gemeentes; 

        public GemeenteRepository(InternshipContext context)
        {
            this.context = context;
            gemeentes = context.Gemeentes;
        }

        public IEnumerable<Gemeente> GetAlleGemeentes()
        {
            return gemeentes;
        }

        public Gemeente FindGemeenteWithId(int id)
        {
            return gemeentes.Find(id);
        }

        public Gemeente FindGemeenteWithStructuur(string structuur)
        {
            return gemeentes.SingleOrDefault(g => g.Structuur.Equals(structuur));
        }
    }
}