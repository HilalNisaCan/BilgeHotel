using Project.MvcUI.PaymentApiTools;
using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Models.PageVm.Reservation
{

    public class ReservationPaymentPageVm
    {
    
        [Display(Name = "Toplam Tutar")]
        public decimal TotalPrice { get; set; }

        public PaymentRequestModel PaymentRequest { get; set; } = new PaymentRequestModel();
    }
}
