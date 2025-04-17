using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    /// <summary>
    /// TC Kimlik doğrulama işlemleri için gerekli bilgileri tutar.
    /// </summary>
    public class KimlikBilgisiDto:IIdentifiablePerson
    {
        /// <summary>
        /// TC Kimlik Numarası
        /// </summary>
        [Required]
        public string IdentityNumber { get; set; } = null!;

        /// <summary>
        /// Müşterinin Adı
        /// </summary>
        [Required]
        public string FirstName { get; set; }=null!;

        /// <summary>
        /// Müşterinin Soyadı
        /// </summary>
        [Required]
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Müşterinin Doğum Yılı
        /// </summary>
        [Required]
        public int BirthYear { get; set; }
        public string PhoneNumber { get; set; } =null!;
    }
}
