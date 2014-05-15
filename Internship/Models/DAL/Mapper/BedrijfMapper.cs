using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Internship.Models.Domain;


namespace Internship.Models.DAL.Mapper
{
    public class BedrijfMapper : EntityTypeConfiguration<Bedrijf>
    {
        public BedrijfMapper()
        {
            
            Property(b => b.Bedrijfsnaam).IsRequired();
            Property(b => b.Telefoon).IsRequired();
            HasRequired(b => b.Adres).WithMany().Map(m => m.MapKey("adres")).WillCascadeOnDelete(true);
            //mappen van de contactpersonen, contactpersoon is verplicht deel van een bedrijf dus heeft een bedrijfsId
            HasMany(b=>b.ContactPersonen).WithRequired().Map(m=>m.MapKey("BedrijfId")).WillCascadeOnDelete(true);
            HasMany(b=>b.Opdrachten).WithRequired().Map(m=>m.MapKey("BedrijfId")).WillCascadeOnDelete(true);
            ToTable("Bedrijf");
        }

    }
}