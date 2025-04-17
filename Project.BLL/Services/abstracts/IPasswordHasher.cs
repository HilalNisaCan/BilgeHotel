using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.abstracts
{
    public interface IPasswordHasher
    {
        string HashPassword(string password); // ✅ Şifreyi hashle
        bool VerifyPassword(string hashedPassword, string providedPassword); // ✅ Şifreyi doğrula
    }
}
