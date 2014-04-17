using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Internship.Models.Domain;

namespace Internship.Models.DAL.Mapper
{
    public class AdresMapper : EntityTypeConfiguration<Adres>
    {
        public AdresMapper()
        {
            ToTable("adres");
            Property(a => a.StraatNaam).IsRequired().HasMaxLength(50);
            Property(a => a.Nummer).IsRequired();
            HasRequired(a => a.Gemeente).WithMany().Map(m => m.MapKey("gemeente"));
            HasKey(a => a.Id);
        }
    }
}