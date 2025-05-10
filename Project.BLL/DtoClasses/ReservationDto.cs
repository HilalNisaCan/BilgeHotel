using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class ReservationDto:BaseDto
    {
        public int? UserId { get; set; } // Giriş yapmış kullanıcı ID'si (anonimse null)

        [Required]
        public int CustomerId { get; set; } // Rezervasyonu yapan müşteri ID'si

        [Required]
        public int RoomId { get; set; } // Rezervasyon yapılan oda ID'si

        public int PaymentId { get; set; } // İlişkili ödeme ID'si

        public int? CampaignId { get; set; } // Uygulanan kampanya ID'si (opsiyonel)

        [Range(0, 100)]
        public decimal ExchangeRate { get; set; } // O günkü döviz kuru

        [Required]
        public DateTime ReservationDate { get; set; } // Rezervasyonun yapıldığı tarih

        [Required]
        public DateTime StartDate { get; set; } // Konaklama başlangıç tarihi

        [Required]
        public DateTime EndDate { get; set; } // Konaklama bitiş tarihi

        public TimeSpan CheckInTime { get; set; } // Giriş saati (örnek: 14:00)

        [Range(1, 10)]
        public int NumberOfGuests { get; set; } // Misafir sayısı

        public ReservationPackage Package { get; set; } // Paket tipi (Tam Pansiyon, Her şey Dahil)

        public decimal TotalPrice { get; set; } // Toplam fiyat

        public decimal DiscountRate { get; set; } // Uygulanan indirim oranı

        public string CurrencyCode { get; set; } // Döviz tipi (örnek: TRY, USD)

        public string? CampaignName { get; set; } // Kampanya adı
     

        public ReservationStatus ReservationStatus { get; set; } // Rezervasyon durumu

        public RoomDto Room { get; set; } = null!;
        public CustomerDto Customer { get; set; } = null!;
    }
}
