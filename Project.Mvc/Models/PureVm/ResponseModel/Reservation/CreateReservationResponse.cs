namespace Project.MvcUI.Models.PureVm.ResponseModel.Reservation
{
    public class CreateReservationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ReservationId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
