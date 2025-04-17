using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class GuestVisitLogDto:BaseDto
    {
        [Required]
        public int CustomerId { get; set; } // Misafir eden müşterinin ID'si

        [Required]
        public int RoomId { get; set; } // Ziyaretin yapıldığı oda ID'si

        [Required, StringLength(100)]
        public string FirstName { get; set; } = null!;// Gelen misafirin adı 

        [Required, StringLength(100)]
        public string LastName { get; set; } = null!; //Gelen misafirin  soyadı

        public string PhoneNumber { get; set; } = null!;

        [Required, StringLength(11, MinimumLength = 11)]
        public string IdentityNumber { get; set; } = null!; // TC Kimlik Numarası

        public DateTime? EntryDate { get; set; } // Giriş tarihi

        public DateTime? ExitDate { get; set; } // Çıkış tarihi (nullable)
        

    }
}
