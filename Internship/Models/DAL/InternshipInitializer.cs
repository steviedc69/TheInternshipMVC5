using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Web;
using System.Web.Security;
using Internship.Models.Domain;
using Internship.Models.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.MySqlClient;

namespace Internship.Models.DAL
{
    public class InternshipInitializer : //DropCreateDatabaseAlways<InternshipContext> 
        DropCreateDatabaseIfModelChanges<InternshipContext>
    {
        private Student s1;
        private Stagebegeleider stagebegeleider;
        protected override void Seed(InternshipContext context)
        {

            try
            {
                /*programmeren, webontwikkeling, mainframe, e-business, mobile, systeembeheer.
                 */


                Specialisatie sp1 = new Specialisatie() {Title = "Programmeren"};
                Specialisatie sp2 = new Specialisatie() {Title = "Webontwikkeling"};
                Specialisatie sp3 = new Specialisatie() {Title = "Mainframe"};
                Specialisatie sp4 = new Specialisatie() {Title = "e-business"};
                Specialisatie sp5 = new Specialisatie() {Title = "Mobile"};
                Specialisatie sp6 = new Specialisatie() {Title = "Systeembeheer"};
                Specialisatie sp7 = new Specialisatie() {Title = "Andere"};
                var specialisaties = new List<Specialisatie>()
                {
                    sp1,
                    sp2,
                    sp3,
                    sp4,
                    sp5,
                    sp6,
                    sp7
                };
                specialisaties.ForEach(c => context.Specialisaties.Add(c));

                Status status1 = new Status(){Naam = "Pending",AlertClass = "alert alert-info",PanelClass = "panel panel-primary"};
                Status status2 = new Status(){Naam = "Afgekeurd",AlertClass = "alert alert-danger",PanelClass = "panel panel-danger"};
                Status status3 = new Status() { Naam = "Stage", AlertClass = "alert alert-success", PanelClass = "panel panel-success" };
                Status status4 = new Status() { Naam = "Project", AlertClass = "alert alert-info", PanelClass = "panel panel-primary" };
                Status status5 = new Status() { Naam = "deels-Toegewezen", AlertClass = "alert alert-info", PanelClass = "panel panel-primary" };
                Status status6 = new Status() { Naam = "Toegewezen", AlertClass = "alert alert-info", PanelClass = "panel panel-primary" };
                var statussen = new List<Status>()
                {
                    status1,
                    status2,
                    status3,
                    status4,
                    status5,
                    status6
                };
                statussen.ForEach(c=>context.Statussen.Add(c));

          
               /* Adres adres = new Adres()
                {
                    StraatNaam = "Straat",
                    Nummer = 5,
                    Gemeente = context.Gemeentes.Find(4)
                };
                */
                s1 = new Student()
                {
                    Naam = "De Cock",
                    Voornaam = "Steven",
                    UserName = "steven.decock.k2806@student.hogent.be",
                    //Adres = adres,
                    Gebdatum = "30-12-1999",
                   

                };
                persistIdentitySeed(s1,context,"paswoord123","Student");
                context.Studenten.Add(s1);

              /*  Bedrijf b1 = new Bedrijf()
                {
                    UserName = "testBedrijf@bedrijf.be",
                    Bedrijfsnaam = "TestBedrijf1",
                    Activiteit = "testing",
                    Bereikbaarheid = "nee",
                    Straat = "bedrijfstraat",
                    Straatnummer = 23,
                    //clientside validation op het telefoonnummer!! 
                    Telefoon = "000/000000",
                    //clientside validation op de url!!
                    Url = "www.bedrijf1.be",
                    Woonplaats = "Aalst"

                };
                ContactPersoon c1 = new ContactPersoon()
                {
                    Naam = "Doe",
                    Voornaam = "John",
                    ContactEmail = "John.Doe@bedrijf.be",
                    ContactTelNr = "000/000000",
                    Functie = "Senior Java Developper"
                };
                b1.AddContactPersoon(c1);
                ContactPersoon c2 = new ContactPersoon()
                {
                    Naam = "Doe",
                    Voornaam = "John",
                    ContactEmail = "John.Doe@bedrijf.be",
                    ContactTelNr = "000/000000",
                    Functie = "Senior Java Developper"
                };
                b1.AddContactPersoon(c2);
                Opdracht o1 = new Opdracht()
                {
                    Title = "Voorstel1",
                    Specialisatie = sp1,
                    Ondertekenaar = c1,
                    StageMentor = c2,
                    Schooljaar = "2014-2015",
                    IsSemester1 = true,
                    IsSemester2 = false,
                    Omschrijving = "Het eerste voorstel van dit bedrijf, programmeren van een back-end java applicatie",
                    Vaardigheden = "De Student moet blalballqskfjmslkfqsmlfkjqslmdkfjslmdfjk"
                };
                b1.AddOpdracht(o1);
                persistIdentitySeed(b1,context,"paswoord123","Bedrijf");
                context.Bedrijven.Add(b1);

                Bedrijf b2 = new Bedrijf()
                {
                    UserName = "testBedrijf2@bedrijf.be",
                    Bedrijfsnaam = "TestBedrijf2",
                    Activiteit = "testing",
                    Bereikbaarheid = "nee",

                    Straat = "bedrijfstraat",
                    Straatnummer = 24,   
                    //clientside validation op het telefoonnummer!! 
                    Telefoon = "000/000001",
                    //clientside validation op de url!!
                    Url = "www.bedrijf2.be",
                 Woonplaats = "Aalst"   

                };
                persistIdentitySeed(b2,context,"paswoord123","Bedrijf");
                context.Bedrijven.Add(b2);
            */
                stagebegeleider = new Stagebegeleider()
              {
                  UserName = "steven.decock@hogent.be",
                  Naam = "De Cock",
                  Voornaam = "Steven",
                  Gsmnummer = "0494888888"
                  
              };
                 persistIdentitySeed(stagebegeleider, context, "paswoord123", "Student");
                context.Stagebegeleiders.Add(stagebegeleider);
                context.SaveChanges();
                //SeedMembership();

            }
            catch (DbEntityValidationException e)
            {
                string s = "Fout creatie database ";
                foreach (var eve in e.EntityValidationErrors)
                {
                    s += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.GetValidationResult());
                    foreach (var ve in eve.ValidationErrors)
                    {
                        s += String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw new Exception(s);
            }
        }

        private void persistIdentitySeed(ApplicationUser us,InternshipContext context,String pasw,String role)
        {
            if (!context.Users.Any(u => u.UserName == us.UserName))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                manager.CreateAsync(us,pasw);
                //manager.AddToRole(us.Id, role);
            }
        }

}

}
