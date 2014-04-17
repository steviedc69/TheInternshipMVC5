using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Internship.Models.Domain;


namespace Internship.Models.DAL.Mapper
{
    public class OpdrachtMapper : EntityTypeConfiguration<Opdracht>
    {
        public OpdrachtMapper()
        {
            HasKey(o => o.Id);
            Property(o => o.Title).IsRequired().HasMaxLength(100);
            Property(o => o.Omschrijving).IsRequired().HasMaxLength(500);
            Property(o => o.Vaardigheden).IsRequired().HasMaxLength(500);
            HasRequired(o => o.Specialisatie).WithMany().Map(m => m.MapKey("specId"));
            HasRequired(o => o.Ondertekenaar).WithMany().Map(m => m.MapKey("Ondertekenaar"));
            HasRequired(o => o.StageMentor).WithMany().Map(m => m.MapKey("StageMentor"));
            Property(o => o.AdminComment).IsOptional().HasMaxLength(500);
            Property(o => o.AantalStudenten).IsRequired();
            ToTable("opdracht");
        }
        

    }
}