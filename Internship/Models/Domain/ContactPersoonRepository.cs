using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Internship.Models.DAL;

namespace Internship.Models.Domain
{
    public class ContactPersoonRepository : IContactPersoonRepository
    {
        private InternshipContext context;
        private DbSet<ContactPersoon> contactpersonen;

        public ContactPersoonRepository(InternshipContext con)
        {
            this.context = con;
            contactpersonen = context.ContactPersonen;
        }

        public void VerwijderContact(ContactPersoon contact)
        {
            contactpersonen.Remove(contact);
            SaveChanges();
        }

        public ContactPersoon FindContactPersoon(int id)
        {
            return contactpersonen.Find(id);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}