using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.MvcUI.Areas.Reservation.Models.PureVm.ResponseModel.ExtraExpense;

namespace Project.MvcUI.Areas.Reservation.Models.PageVm
{
    public class CheckOutDetailModel
    {
        public int ReservationId { get; set; }
        public string CustomerFullName { get; set; } = null!;
        public string RoomNumber { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReservationPackage Package { get; set; }
        public decimal TotalPrice { get; set; }

        // Sadece Tam Pansiyon kullanıcıları için
        public List<ExtraExpenseModel> ExtraExpenses { get; set; } = new();
    }

}
