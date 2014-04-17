using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Internship.Models.DAL.Mapper
{

    public class IdentityUserMapper : EntityTypeConfiguration<IdentityUser>
    {
        public IdentityUserMapper()
        {
            ToTable("aspnetusers");
        }
    }
    public class IdentityUserRoleMapper : EntityTypeConfiguration<IdentityUserRole>
    {
        public IdentityUserRoleMapper()
        {
            HasKey(r => new {UserId = r.UserId, RoleId = r.RoleId});
            ToTable("aspnetuserrole");
        }
        
    }

    public class IdentityUserLoginMapper : EntityTypeConfiguration<IdentityUserLogin>
    {
        public IdentityUserLoginMapper()
        {
            HasKey(i => new {UserId = i.UserId, LoginProvider = i.LoginProvider, ProviderKey = i.ProviderKey});
            HasRequired(u => u.User);
            ToTable("aspnetuserlogins");
        }
    }

    public class IdentityUserClaimMapper : EntityTypeConfiguration<IdentityUserClaim>
    {
        public IdentityUserClaimMapper()
        {
            HasRequired(u => u.User);
            ToTable("aspnetuserclaims");
        }
    }

    public class IdentityRoleMapper : EntityTypeConfiguration<IdentityRole>
    {
        public IdentityRoleMapper()
        {
            ToTable("aspnetroles");
        }
    }
}