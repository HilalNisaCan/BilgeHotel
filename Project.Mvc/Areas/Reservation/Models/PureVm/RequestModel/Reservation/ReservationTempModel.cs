using Project.Entities.Enums;

namespace Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Reservation
{  /// <summary>
   /// Ödeme tamamlanmadan önce geçici olarak tutulan rezervasyon verilerini temsil eder.
   /// Session veya TempData ile taşınır.
   /// </summary>
    public class ReservationTempModel
    {
        public int RoomId { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }

        public RoomType RoomType { get; set; }
        public ReservationPackage Package { get; set; }

        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        public int Duration { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal TotalPrice { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
