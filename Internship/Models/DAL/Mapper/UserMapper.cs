using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Internship.Models.Domain;

namespace Internship.Models.DAL.Mapper
{
    public class UserMapper : EntityTypeConfiguration<ApplicationUser>
    {
        public UserMapper()
        {
            HasKey(u => u.Id);
            Property(u => u.UserName).IsRequired();
            //Property(u => u.Emailadres).IsRequired();
            
            ToTable("aspnetusers");
        }
        

    }
}