namespace Project.MvcUI.Models.PageVm.Reservation
{
    public class ReservationListVm
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public string CustomerFullName { get; set; } // DTO'dan FirstName + LastName birleşimi
        public string CustomerPhone { get; set; } // DTO'dan PhoneNumber
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string ReservationStatusName { get; set; }
        public decimal TotalPrice { get; set; }
        public string ReservationPackageName { get; set; }
    }
}
