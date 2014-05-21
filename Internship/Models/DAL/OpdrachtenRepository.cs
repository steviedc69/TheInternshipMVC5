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


        public IEnumerable<Opdracht> GeefActieveOpdrachten()
        {
            int year = DateTime.Now.Year;
            
            return Opdrachten.Where(o => o.Status.Id == 3 || o.Status.Id == 5 ).OrderBy(o=>o.Schooljaar);//&& o.SchoolJaarFirstInt()>=year 
               // && o.SchoolJaarSecondInt()>year).OrderBy(o => o.Schooljaar);
        }

        public Opdracht SearchOpdracht(string title, string company, string ondertekenaar, string specialisatie)
        {

            String[] s = ondertekenaar.Split(' ');
            string voorn = s[1];
            string achtern = s[0];
            IEnumerable<Opdracht> opdrachts = Opdrachten.Where(o => o.Title.Equals(title));

            Opdracht opdracht =
                opdrachts.SingleOrDefault(
                    o => o.Bedrijf.Bedrijfsnaam.Equals(company) && o.Specialisatie.Title.Equals(specialisatie));
            if (opdracht.Ondertekenaar != null)
            {
                if (opdracht.Ondertekenaar.Voornaam.Equals(voorn) && opdracht.Ondertekenaar.Naam.Equals(achtern))
                {
                    return opdracht;
                }
            }
            return null;


        }

        public IEnumerable<Opdracht> GeefStageOpdrachten()
        {
            return Opdrachten.Where(o=>o.Status.Id==3||o.Status.Id==5).OrderBy(o=>o.Schooljaar);
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