using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

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
    }
}