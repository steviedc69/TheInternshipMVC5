using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using Internship.Models.Domain;
using Microsoft.Ajax.Utilities;
using Microsoft.SqlServer.Server;

namespace Internship.Models.Domain
{
    public class Bedrijf : ApplicationUser
    {


        public String Bedrijfsnaam { get; set; }
        public String Url { get; set; }
        public virtual Adres Adres { get; set; }
        public String Telefoon { get; set; }
        public Boolean Openbaarvervoer { get; set; }
        public Boolean PerAuto { get; set; }
   
        public String Activiteit { get; set; }


        public virtual ICollection<ContactPersoon> ContactPersonen { get; private set; }
        [InverseProperty("Bedrijf")]
        public virtual ICollection<Opdracht> Opdrachten { get; set; }

        private ISearchStragegy SearchStragegy { get; set; }
        public String ImageUrl { get; set; }

        public Bedrijf()
        {
            ContactPersonen = new List<ContactPersoon>();
            Opdrachten = new List<Opdracht>();
            ImageUrl = "/Images/HG.gif";
        }



        public void AddContactPersoon(ContactPersoon persoon)
        {
            this.ContactPersonen.Add(persoon);
        }

        public void RemoveContactPersoon(ContactPersoon persoon)
        {
            this.ContactPersonen.Remove(persoon);
        }

        public void AddOpdracht(Opdracht opdracht)
        {
            this.Opdrachten.Add(opdracht);
        }

        public void RemoveOpdracht(Opdracht opdracht)
        {
            this.Opdrachten.Remove(opdracht);
        }

        public ContactPersoon FindContactPersoon(String voornaamNaam)
        {
            ContactPersoon c = null;
            foreach (var contact in ContactPersonen)
            {
                if (contact.ToString().Equals(voornaamNaam))
                {
                    c = contact;
                    return c;
                }
            }
            return c;
        }

        public ContactPersoon FindContactPersoonById(int id)
        {
            ContactPersoon contact = null;
            foreach (ContactPersoon contactPersoon in ContactPersonen)
            {
                if (contactPersoon.Id == id)
                {
                    contact = contactPersoon;
                }
            }
            return contact;

        }

        public Opdracht FindOpdracht(int id)
        {
            Opdracht opdracht = null;
            foreach (Opdracht op in Opdrachten)
            {
                if (op.Id == id)
                {
                    opdracht = op;
                }
            }
            return opdracht;
        }

        public List<Opdracht> FindOpdrachtWithContact(ContactPersoon persoon)
        {
            List<Opdracht> opdrachten = new List<Opdracht>();
            foreach (Opdracht opdracht in Opdrachten)
            {
                if (opdracht.IsContactFromOpdracht(persoon))
                {
                    opdrachten.Add(opdracht);
                }
            }
            return opdrachten;
        }

        public Boolean IsOpdrachtenWithContact(ContactPersoon persoon)
        {
            return FindOpdrachtWithContact(persoon).Count != 0;
        }

        public IList<Opdracht> GetArchive(int year)
        {
            IList<Opdracht> opdracten = new List<Opdracht>();
            foreach (Opdracht o in Opdrachten)
            {
                int? y = o.SchoolJaarSecondInt();
                if (y != null && y < year)
                {
                    opdracten.Add(o);
                }
            }
            return opdracten;
        }

        public IList<Opdracht> GetListOfActiveOpdrachten(int year, int month)
        {
            IList<Opdracht> opdrachts = new List<Opdracht>();

            foreach (Opdracht o in Opdrachten)
            {
                if (o.SchoolJaarSecondInt() != null && o.SchoolJaarSecondInt() == year && month < 7 &&
                    o.IsSemester2 == true)
                {
                    opdrachts.Add(o);
                }
                if (o.SchoolJaarFirstInt() >= year)
                {
                    opdrachts.Add(o);
                }

            }
            return opdrachts;
        }

        public void AddStrategy(ISearchStragegy strategy)
        {
            this.SearchStragegy = strategy;
        }

        public IList<Opdracht> SearchResult(IList<Opdracht> opdrachts, String search)
        {
            if (SearchStragegy != null)
            {
                return SearchStragegy.Search(opdrachts, search);
            }
            return null;
        }

        public override string ToString()
        {
            return Bedrijfsnaam+" "+Adres.Gemeente.Naam;
        }
    }
}
