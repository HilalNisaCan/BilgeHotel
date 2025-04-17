using Project.BLL.DtoClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.abstracts
{
    /// <summary>
    /// Kimlik doğrulama işlemlerini gerçekleştiren servis arayüzü.
    /// </summary>
    public interface IIdentityValidationService
    {
        /// <summary>
        /// T.C. Kimlik doğrulaması yapar.
        /// </summary>
        /// <param name="dto">Kimlik bilgilerini içeren DTO</param>
        /// <returns>Doğrulama başarılıysa true, değilse false</returns>
        Task<bool> VerifyAsync(KimlikBilgisiDto dto);
    }
}
