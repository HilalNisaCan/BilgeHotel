using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class Reservation:BaseEntity
    {
        // Kullanıcıya ait FK (üye olmayanlar için null olabilir)
        public int? UserId { get; set; }

        // Rezervasyon sahibine ait müşteri FK
        public int CustomerId { get; set; }

        // Oda ile ilişki
        public int RoomId { get; set; }

        // Ödeme ile birebir ilişki
     //   public int PaymentId { get; set; }

        // Opsiyonel kampanya ilişkisi
        public int? CampaignId { get; set; }

        // O günkü kur değeri (ExchangeRate tablosundan alınır)
        public decimal ExchangeRate { get; set; }

        // Tarih Bilgileri
        public DateTime ReservationDate { get; set; } // Rezervasyonun oluşturulma tarihi
        public DateTime StartDate { get; set; }       // Giriş tarihi
        public DateTime EndDate { get; set; }         // Çıkış tarihi
        public TimeSpan CheckInTime { get; set; }     // Tahmini giriş saati

        // Kişi Sayısı
        public int NumberOfGuests { get; set; }

        // Rezervasyon paketi: Tam pansiyon / Herşey Dahil vs.
        public ReservationPackage Package { get; set; }

        // Fiyatlandırma bilgileri
        public decimal TotalPrice { get; set; }
        public decimal DiscountRate { get; set; }
        public string? CurrencyCode { get; set; }

        // Rezervasyon durumu: Bekliyor, Onaylandı, İptal Edildi vs.
        public ReservationStatus ReservationStatus { get; set; }

        //relational properties

        public virtual User User { get; set; } = null!; // Rezervasyonu yapan kullanıcı
        public virtual Customer Customer { get; set; } = null!; // Müşteri
        public virtual Room Room { get; set; } = null!; // Oda
        public virtual Payment Payment { get; set; } = null!; // Ödeme
        public virtual Campaign Campaign { get; set; } = null!; // Kampanya
        public virtual EarlyReservationDiscount EarlyReservationDiscount { get; set; } = null!; // Birebir ilişki ✔️
        public virtual ICollection<ExtraExpense> ExtraExpenses { get; set; } = null!;// Oda dışı harcamalar
        public virtual ICollection<Review> Reviews { get; set; } = null!; // Yorumlar
        public virtual ICollection<Order> Orders { get; set; } = null!;  // Siparişler (örnek: oda servisi, bar siparişi)

    }


}

