using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Internship.Models.Domain;

namespace Internship.Models.DAL.Mapper
{
    public class StudentMapper : EntityTypeConfiguration<Student>
    {
        public StudentMapper()
        {
           // HasKey(s => s.Emailadres);
            Property(s => s.Naam).HasMaxLength(30).IsOptional();
            Property(s => s.Voornaam).HasMaxLength(20).IsOptional();
            HasMany(s => s.Favorites).WithMany(o => o.StudentenFavorites).Map(s =>
            {
                s.ToTable("Favorites");
                s.MapLeftKey("StudentId");
                s.MapRightKey("OpdrachtId");
            });
            HasOptional(s => s.Adres).WithMany().Map(m => m.MapKey("adresId")).WillCascadeOnDelete(true);
            HasOptional(s=>s.StageOpdracht).WithMany(o=>o.StageStudenten).Map(s=>s.MapKey("StageId")).WillCascadeOnDelete(false);
            ToTable("Student");
        }
    }
}