using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{

    /// <summary>
    /// Müşteri bazlı raporlama bilgilerini taşıyan DTO'dur.
    /// Yönetici panelinde müşteri performansları, harcama durumu ve rezervasyon özetleri bu model ile gösterilir.
    /// </summary>
    public class CustomerReportDto:BaseDto
    {
        /// <summary>
        /// Müşteri ID'si
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Müşterinin tam adı (Ad + Soyad)
        /// </summary>
        public string FullName { get; set; } = null!;

        /// <summary>
        /// TC Kimlik Numarası
        /// </summary>
        public string IdentityNumber { get; set; }= null!;

        /// <summary>
        /// Telefon numarası
        /// </summary>
        public string PhoneNumber { get; set; } = null!;    

        /// <summary>
        /// Toplam yaptığı rezervasyon sayısı
        /// </summary>
        public int TotalReservationCount { get; set; }

        /// <summary>
        /// Tüm rezervasyonlardan elde edilen toplam harcama tutarı
        /// </summary>
        public decimal TotalSpent { get; set; }

        /// <summary>
        /// Müşterinin sahip olduğu sadakat puanı
        /// </summary>
        public int LoyaltyPoints { get; set; }

        /// <summary>
        /// Müşterinin yaptığı en son rezervasyonun bitiş tarihi
        /// </summary>
        public DateTime? LastReservationDate { get; set; } 

        /// <summary>
        /// Kaç rezervasyonda kampanya kullanılmış
        /// </summary>
        public int CampaignUsageCount { get; set; }

        public int ReservationCount { get; set; } // ✅ EKLENECEK

        public int CampaignCount { get; set; } // ✅ EKLENECEK

    }
}
