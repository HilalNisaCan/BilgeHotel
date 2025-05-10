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
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string IdentityNumber { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int TotalReservationCount { get; set; }
        public decimal TotalSpent { get; set; }
        public int LoyaltyPoints { get; set; }
        public DateTime? LastReservationDate { get; set; }
        public int CampaignUsageCount { get; set; }
        public int ReservationCount { get; set; }
        public int CampaignCount { get; set; }
        public List<ReservationDto> PastReservations { get; set; } = new();

        // ✅ Eklenen alanlar
        public string? UserId { get; set; }
        public bool HasUser => !string.IsNullOrEmpty(UserId);
        public List<ReservationDto> UpcomingReservations { get; set; } = new();
        public List<ReservationDto> CurrentStays { get; set; } = new();

    }
}
