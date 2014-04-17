using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Internship.Models.Domain;


namespace Internship.Models.DAL.Mapper
{
    public class ContactPersoonMapper : EntityTypeConfiguration<ContactPersoon>
    {
        public ContactPersoonMapper()
        {
            HasKey(c => c.Id);
            Property(c => c.Naam).IsRequired();
            Property(c => c.ContactEmail).IsRequired();
            Property(c => c.ContactTelNr).IsRequired();
        }
    }
}