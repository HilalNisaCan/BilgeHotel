using Project.Entities.Enums;

namespace Project.MvcUI.Areas.Reservation.Models.PageVm
{
    public class ReservationCheckInOutModel
    {
        public int ReservationId { get; set; }
        public string CustomerFullName { get; set; } = null!;
        public string RoomNumber { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReservationStatus ReservationStatus { get; set; }
        public ReservationPackage Package { get; set; }
        public bool IsTodayCheckIn => StartDate.Date == DateTime.Today;
        public bool IsTodayCheckOut => EndDate.Date == DateTime.Today;
    }
}
