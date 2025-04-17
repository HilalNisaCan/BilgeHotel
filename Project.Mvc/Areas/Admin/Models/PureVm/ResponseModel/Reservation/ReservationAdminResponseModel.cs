using Project.Entities.Enums;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Reservation
{
    public class ReservationAdminResponseModel
    {
        public int Id { get; set; }

        // Müşteri bilgileri
        public string? CustomerFullName { get; set; }
        public string? IdentityNumber { get; set; }
        public string? PhoneNumber { get; set; }

        // Oda bilgisi
        public string RoomInfo { get; set; } = null!;

        // Konaklama detayları
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan? CheckInTime { get; set; } // optional

        public int NumberOfGuests { get; set; }
        public string? Package { get; set; }

        // Kampanya bilgisi
        public string? CampaignName { get; set; }
        public decimal DiscountRate { get; set; }

        // Ücret bilgileri
        public decimal TotalPrice { get; set; }
        public string? CurrencyCode { get; set; }

        // Rezervasyon durumu ve zamanı
        public ReservationStatus ReservationStatus { get; set; }
        public DateTime ReservationDate { get; set; }
    }
}
