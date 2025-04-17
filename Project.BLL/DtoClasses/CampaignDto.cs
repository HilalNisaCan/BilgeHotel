using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class CampaignDto:BaseDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; } // Kampanyanın adı

        [StringLength(250)]
        public string? ProductImagePath { get; set; }
        [Required]
        public ReservationPackage Package { get; set; } // Kampanyanın uygulandığı paket

        [Range(0, 100)]
        public decimal DiscountPercentage { get; set; } // İndirim yüzdesi

        [Range(1, 365)]
        public int ValidityDays { get; set; } // Kampanyanın geçerlilik süresi (gün)

        [Required]
        public DateTime StartDate { get; set; } // Başlangıç tarihi

        [Required]
        public DateTime EndDate { get; set; } // Bitiş tarihi

        public CampaignStatus Status { get; set; } // Kampanyanın durumu

        public CampaignType CampaignType { get; set; } // Kampanya tipi (örn. Yaz indirimi)

        public bool IsActive { get; set; } // Aktiflik durumu
    }
}
