using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace OnlineBikeStore
{
 
    public static class Password
    {
        //Method to convert password in hash form
        public static string EncryptPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {                
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));                
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}