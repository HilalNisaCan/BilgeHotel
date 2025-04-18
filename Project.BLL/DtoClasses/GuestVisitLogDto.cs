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
        public int CustomerId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(11)]
        public string IdentityNumber { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ExitDate { get; set; }
        public DateTime LastReservationDate { get; set; }

        // Yeni ekleyebileceğin alanlar (opsiyonel, önerilen):
        public int TotalReservationCount { get; set; }
        public decimal TotalSpent { get; set; }
        public int LoyaltyPoints { get; set; }
        public int CampaignUsageCount { get; set; }


    }
}
