using Microsoft.AspNetCore.Identity;
using Project.BLL.Services.abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Concretes
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            string hashedProvidedPassword = HashPassword(providedPassword);
            return hashedPassword == hashedProvidedPassword;
        }
    }

}
