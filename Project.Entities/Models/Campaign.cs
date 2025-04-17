using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class Campaign:BaseEntity
    {
        // Kampanya adı (örnek: "Yılbaşı Tatili %15 İndirim")
        public string? Name { get; set; }

        // Kampanyaya ait görsel (UI gösterimi için)
        public string? ProductImagePath { get; set; }

        // Bu kampanya hangi rezervasyon paketinde geçerli? (örn: Herşey Dahil, Oda Kahvaltı)
        public ReservationPackage Package { get; set; }

        // Kampanya indirim oranı (%)
        public decimal DiscountPercentage { get; set; }

        // Kampanya ne kadar önceden yapılırsa geçerli? (örn: min. 5 gün önceden rezervasyon)
        public int ValidityDays { get; set; }

        // Başlangıç ve bitiş tarihleri
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Kampanya durumu (Planlandı, Aktif, Sona Erdi vb.)
        public CampaignStatus CampaignStatus { get; set; }

        // Kampanya tipi (örn: Genel Kampanya, Erken Rezervasyon, Sadakat vs.)
        public CampaignType CampaignType { get; set; }

        // Sistemde aktif mi? (bool flag)
        public bool IsActive { get; set; }


        //relational properties
        public virtual required ICollection<Reservation> Reservations { get; set; } // 1-N: Bir kampanya birden çok rezervasyona uygulanabilir
        public virtual required ICollection<EarlyReservationDiscount> EarlyReservationDiscounts { get; set; }  // ✅ Kampanyaya bağlı erken rezervasyon indirimleri (1 - N)
    }
}

