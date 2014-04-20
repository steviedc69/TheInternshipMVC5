using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Internship.Models.Domain;

namespace Internship.ViewModels
{
    public static class Mapper
    {
        //extension methods


        //mapper methods van contactpersoon
        public static ContactModel ConvertToContactCreateModel(this ContactPersoon contact, String bedrijfId)
        {
            //teruggeven van contactCreateModel
            return new ContactModel()
            {
                //opvullen van viewmodel
                BedrijfsId = bedrijfId,
                id = contact.Id,
                Naam = contact.Naam,
                Voornaam = contact.Voornaam,
                Functie = contact.Functie,
                ContactEmail = contact.ContactEmail,
                ContactTelNr = contact.ContactTelNr,
                GsmNummer = contact.GsmNummer
            };
        }

        public static void UpdateContact(this ContactPersoon contactToEdit, ContactModel contact)
        {
            //updaten van attributen in contact in bedrijf
            contactToEdit.Naam = contact.Naam;
            contactToEdit.Voornaam = contact.Voornaam;
            contactToEdit.ContactEmail = contact.ContactEmail;
            contactToEdit.ContactTelNr = contact.ContactTelNr;
            contactToEdit.Functie = contact.Functie;
            contactToEdit.GsmNummer = contact.GsmNummer;
        }





        //mapper methods opdracht.
        


        //aanmaken van viewmodel voor het tonen van de opdrachten in een lijst.

        /*
        public static OpdrachtIndexViewModel ConvertToOpdrachtIndexViewModel(this Opdracht opdracht)
        {
            return new OpdrachtIndexViewModel()
            {
                
            };
        }
        */


        //updaten van een opdracht door opvragen van viewmodel van Opdracht
        public static void UpdateOpdracht(this Opdracht opdrachtToEdit,  CreateOpdrachtViewModel model)
        {
            //updaten van attributen in contact in bedrijf
            opdrachtToEdit.AantalStudenten = (int)model.AantalStudenten.SelectedValue;
            if (model.SemesterLijst.SelectedValue.Equals("Semester 1 en 2"))
            {
                opdrachtToEdit.IsSemester1 = true;
                opdrachtToEdit.IsSemester2 = true;
            }
            else if (model.SemesterLijst.SelectedValue.Equals("Semester 1"))
            {
                opdrachtToEdit.IsSemester1 = true;
                opdrachtToEdit.IsSemester2 = false;
            }
            else
            {
                opdrachtToEdit.IsSemester1 = false;
                opdrachtToEdit.IsSemester2 = true;
            }

            opdrachtToEdit.Omschrijving = model.OpdrachtViewModel.Omschrijving;
            opdrachtToEdit.Schooljaar = model.SchooljaarSelectList.SelectedValue.ToString();
            opdrachtToEdit.Title = model.OpdrachtViewModel.Title;
            opdrachtToEdit.Vaardigheden = model.OpdrachtViewModel.Vaardigheden;
            
            //Deze moeten nog worden aangevuld
            
            //opdrachtToEdit.Adres = 
            //opdrachtToEdit.Ondertekenaar =
            //opdrachtToEdit.Specialisatie =
            //opdrachtToEdit.StageMentor =
            


        }
    }
}