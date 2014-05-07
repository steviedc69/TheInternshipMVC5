using System;
using System.Collections.Generic;
using System.Security.Policy;
using Internship.Models.Domain;
using Microsoft.SqlServer.Server;

namespace Internship.Models.Domain
{
    public class Bedrijf : ApplicationUser
    {


        public String Bedrijfsnaam { get; set; }
        public String Url { get; set; }
        public virtual Adres Adres { get; set; }
        public String Telefoon { get; set; }
        public String Bereikbaarheid { get; set; }
        public String Activiteit { get; set; }


        public virtual ICollection<ContactPersoon>ContactPersonen { get; private set; } 
        public virtual ICollection<Opdracht>Opdrachten { get; set; } 

        public Bedrijf()
        {
           ContactPersonen = new List<ContactPersoon>(); 
           Opdrachten = new List<Opdracht>(); 
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

    }
}
