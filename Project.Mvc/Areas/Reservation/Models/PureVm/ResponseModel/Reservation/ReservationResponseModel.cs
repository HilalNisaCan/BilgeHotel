namespace Project.MvcUI.Areas.Reservation.Models.PureVm.ResponseModel.Reservation
{
    public class ReservationResponseModel
    {
        public string? RoomName { get; set; }
        public string? CustomerFullName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string? PackageName { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountRate { get; set; }
       
    }
}
