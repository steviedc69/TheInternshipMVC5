using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Internship.Models.Domain;

namespace Internship.Models.DAL.Mapper
{
    public class GemeenteMapper : EntityTypeConfiguration<Gemeente>
    {
        public GemeenteMapper()
        {
            ToTable("gemeente");
            Property(g => g.Naam).IsRequired();
            Property(g => g.Postcode).IsRequired();
            Property(g => g.Structuur).IsRequired();
            Property(g => g.Up).IsRequired();
            HasKey(g => g.Id);
        }
    }
}