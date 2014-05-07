using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Internship.Models.DAL.Mapper;
using Internship.Models.Domain;
using Internship.Models.DAL.Mapper;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;

namespace Internship.Models.DAL
{
    //hier zullen waarschijnlijk nog dingen moeten aangevuld worden
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class InternshipContext : IdentityDbContext<ApplicationUser>
    {
        public InternshipContext() : base("internshipdb")
        {
            
        }

        //public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Bedrijf> Bedrijven { get; set; }
        public DbSet<Student> Studenten { get; set; }
        public DbSet<Stagebegeleider> Stagebegeleiders { get; set; }
        //public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Opdracht>Opdrachten { get; set; } 
        public DbSet<Specialisatie> Specialisaties { get; set; }
        public DbSet<ContactPersoon> ContactPersonen { get; set; }
        public DbSet<Gemeente> Gemeentes { get; set; }
        public DbSet<Adres> Adressen { get; set; }
        public DbSet<Status> Statussen { get; set; } 
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new BedrijfMapper());
            modelBuilder.Configurations.Add(new StudentMapper());
            modelBuilder.Configurations.Add(new ContactPersoonMapper());
            modelBuilder.Configurations.Add(new StageBegeleiderMapper());
            modelBuilder.Configurations.Add(new OpdrachtMapper());
            modelBuilder.Configurations.Add(new SpecialisatieMapper());
            modelBuilder.Configurations.Add(new IdentityUserRoleMapper());
            modelBuilder.Configurations.Add(new IdentityUserLoginMapper());
            modelBuilder.Configurations.Add(new IdentityUserClaimMapper());
            modelBuilder.Configurations.Add(new IdentityRoleMapper());
            modelBuilder.Configurations.Add(new UserMapper());
            modelBuilder.Configurations.Add(new IdentityUserMapper());
            modelBuilder.Configurations.Add(new AdresMapper());
            modelBuilder.Configurations.Add(new GemeenteMapper());

        }
       
    }
 
}