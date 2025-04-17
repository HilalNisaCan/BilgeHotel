using Project.BLL.DtoClasses;
using Project.MvcUI.PaymentApiTools;

namespace Project.MvcUI.Models.PageVm.Payment
{
    public class PaymentPageVm
    {
        public ReservationDto Reservation { get; set; }

        public PaymentRequestModel PaymentRequest { get; set; }
    }
}
