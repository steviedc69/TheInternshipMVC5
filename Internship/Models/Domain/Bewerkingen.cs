using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using Internship.ViewModels;

namespace Internship.Models.Domain
{
    public static class Bewerkingen
    {
        //klasse die belangerijke bewerkingen op zich neemt, zoals omvormen van passwd
        
        /// <summary>
        /// comparePasswd : vergelijken van passwoorden
        /// </summary>
        /// <param name="passwd">hash value passwd uit database</param>
        /// <param name="comparePasswd">String value uit login pagina</param>
        /// <returns>true or false</returns>
        public static bool ComparePasswd(String passwd, String comparePasswd)
        {
            String hashPaswd = Sha256_hash(comparePasswd);
            if (passwd.Equals(hashPaswd))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static String GetEncryptedPasswd(String value)
        {
            return Sha256_hash(value);
        }

        private static String Sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static int? TryParseString(String s)
        {
            int num;
            bool result = Int32.TryParse(s, out num);
            return num;
        }

        private static String MakeBody(String title,String schooljaar, String omschijving,String specialisatie)
        {
            return "Uw opdracht " + title + " voor " + schooljaar + "<br/>" + "Met volgende omschrijving : " +
                   omschijving + "<br/>" +
                   "En specialisatie : " + specialisatie +
                   ".<br/> Werd succesvol aangemaakt, uw aanvraag wordt door onze administratie verwerkt<br/>" +
                   "en u wordt zo snel mogelijk op de hoogte gebracht." + "<br/><br/>" + "Met vriendelijke groet,<br/><br/>" +
                   "IDev<br/><br/>";


        }

        public static void SendMail(String email, OpdrachtViewModel model)
        {
            MailMessage message = new MailMessage();
            message.To.Add(email);
            message.From = new MailAddress("noreply@idev.be");
            message.CC.Add("stevie.dc.SDC@gmail.com");
            message.Subject = "Nieuwe aanvraag " + model.Title;
            message.Body = MakeBody(model.Title, model.Schooljaar, model.Omschrijving, model.Specialisatie);
            message.IsBodyHtml = true;
            

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            ("stevie.dc.SDC@gmail.com", "Samsungs4");// Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.Send(message);
         

        }
        
         public static List<String> MakeSchooljaarSelectList()
        {
            List<String> lijstSchooljaren = new List<string>();
            DateTime date = DateTime.Now;
            int year = date.Year;
            if (date.Month < 2)
            {
                for (int i = -1; i < 5; i++)
                {
                    String schooljaar = (year + i) + " - " + (year + i + 1);
                    lijstSchooljaren.Add(schooljaar);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    
                    String schooljaar = (year + i) + " - " + (year + i + 1);
                    lijstSchooljaren.Add(schooljaar);
                }
            }

            return lijstSchooljaren;
        }
    }
    }
