using Project.MvcUI.PaymentApiTools;
using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Models.PageVm.Reservation
{

    public class ReservationPaymentPageVm
    {
        public int ReservationId { get; set; }

        [Display(Name = "Toplam Tutar")]
        public decimal TotalPrice { get; set; }

        public PaymentRequestModel PaymentRequest { get; set; } = new PaymentRequestModel();
    }
}
