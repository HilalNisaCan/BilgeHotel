using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Login
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Kullanıcı adı alanı boş bırakılamaz.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        public string? Password { get; set; }
    }
}
