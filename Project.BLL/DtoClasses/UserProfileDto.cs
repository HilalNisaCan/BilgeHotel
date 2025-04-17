using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class UserProfileDto : BaseDto,IIdentifiablePerson
    {

        [Required, StringLength(50)]
        public string FirstName { get; set; } = null!; // Kullanıcının adı

        [Required, StringLength(50)]
        public string LastName { get; set; } = null!; // Kullanıcının soyadı

        public string PhoneNumber { get; set; }=null!;

        [StringLength(200)]
        public string? Address { get; set; } // Kullanıcının adres bilgisi

        [StringLength(100)]
        public string? City { get; set; } // Şehir

        [StringLength(100)]
        public string? Country { get; set; } // Ülke

        [StringLength(50)]
        public string? Nationality { get; set; } // Uyruk

        [StringLength(10)]
        public string? Gender { get; set; } // Cinsiyet

        [StringLength(11)]
        public string IdentityNumber { get; set; } = null!;// TC kimlik numarası

        public DateTime? BirthDate { get; set; } // Doğum tarihi

        public int UserId { get; set; } // İlişkili kullanıcı ID'si
      
    }
}
