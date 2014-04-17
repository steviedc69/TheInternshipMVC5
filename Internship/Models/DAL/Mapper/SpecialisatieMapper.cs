using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Internship.Models.Domain;


namespace Internship.Models.DAL.Mapper
{
    public class SpecialisatieMapper : EntityTypeConfiguration<Specialisatie>
    {
        public SpecialisatieMapper()
        {
            HasKey(s => s.Id);
            Property(s => s.Title).IsRequired();
           //HasMany(s=>s.Opdrachten).WithRequired().Map(m=>m.MapKey("opdrachtId")).WillCascadeOnDelete(false);
            ToTable("specialisatie");
        }

    }
}