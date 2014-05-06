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
            ToTable("Student");
        }
    }
}