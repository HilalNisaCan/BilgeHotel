using Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Reservation;
using Project.MvcUI.Models.PureVm.ResponseModel.Room;

namespace Project.MvcUI.Areas.Reservation.Models.PageVm
{
    public class ReservationPageVm
    {
        public CreateReservationRequestModel ReservationForm { get; set; } 
        public List<RoomResponseModel> AvailableRooms { get; set; } 
    }
}
