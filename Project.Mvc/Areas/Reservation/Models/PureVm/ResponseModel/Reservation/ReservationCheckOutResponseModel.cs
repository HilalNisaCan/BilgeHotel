namespace Project.MvcUI.Areas.Reservation.Models.PureVm.ResponseModel.Reservation
{
    public class ReservationCheckOutResponseModel
    {
        public int ReservationId { get; set; }
        public string CustomerFullName { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Package { get; set; }
    }
}
