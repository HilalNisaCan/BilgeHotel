using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Register
{
    public class RegisterRequestModel
    {
        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, DataType(DataType.Password)]
        [MinLength(6)]
        public string? Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string? ConfirmPassword { get; set; }
    }
}
