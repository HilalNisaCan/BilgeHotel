using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Login
{
    public class ReservationForgotPasswordViewModel
    {
        [Required(ErrorMessage = "E-posta alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = null!;
    }
}
