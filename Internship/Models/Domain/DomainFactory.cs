using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using Internship.Models.DAL;
using Internship.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Internship.Models.Domain
{
    public static class DomainFactory
    {

        public static Bedrijf createBedrijf(String bedrijfnaam, String activiteit, String bereikbaarheid,
            String url, String straat, int nummer, String woonplaats, String telefoon, String email,IGemeenteRepository gemeenteRepo)
        {
            Gemeente g = gemeenteRepo.FindGemeenteWithStructuur(woonplaats);
            Adres adres = AddAdres(straat, nummer, g);

            Bedrijf bedrijf = new Bedrijf()
            {
                Bedrijfsnaam = bedrijfnaam,
                Activiteit = activiteit,
                Bereikbaarheid = bereikbaarheid,
                Url = url,
                Telefoon = telefoon,
                UserName = email,
                Adres = adres
            };
            
            
            return bedrijf;
        }

        public static void CreateNewStudent(String eMail, IUserRepository repository,
            IStudentRepository studentRepository)
        {
            if (!repository.UserExist(eMail))
            {
                String[] split = Regex.Split(eMail, "@");
                String[] naamSplit = Regex.Split(eMail, ".");
                String voornaam = naamSplit[0];
                String achternaam = naamSplit[1];
                String pasw = split[0];
                Student student = new Student()
                {
                    UserName = eMail,
                    Voornaam = voornaam,
                    Naam = achternaam

                };

                repository.CreateAsyncUser(student, pasw);
                studentRepository.AddStudent(student);
            }
        }

        public static Opdracht CreateOpdracht(OpdrachtViewModel viewModel,Bedrijf bedrijf,ISpecialisatieRepository repo)
        {
            bool sem1 = false;
            bool sem2 = false;
            if (viewModel.Semesters.Equals("Semester 1"))
            {
                sem1 = true;
            }
            else if (viewModel.Semesters.Equals("Semester 2"))
            {
                sem2 = true;
            }
            else
            {
                sem1 = true;
                sem2 = true;
            }

        Opdracht o = new Opdracht()
            {
                AantalStudenten = viewModel.AantalStudenten,
                Schooljaar = viewModel.Schooljaar,
                Omschrijving = viewModel.Omschrijving,
                Vaardigheden = viewModel.Vaardigheden,
                IsSemester1 = sem1,
                IsSemester2 = sem2,
                Title = viewModel.Title,
                //Ondertekenaar = bedrijf.FindContactPersoon(viewModel.ContractOndertekenaar),
                //StageMentor = bedrijf.FindContactPersoon(viewModel.StageMentor),
                Specialisatie = repo.FindSpecialisatieNaam(viewModel.Specialisatie)
                
            };
            return o;
        }
        public static Adres AddAdres(String straat, int nummer, Gemeente gemeente)
        {
            Adres adres = new Adres()
            {
                Gemeente = gemeente,
                StraatNaam = straat,
                Nummer = nummer
            };
            return adres;
        }
}
}