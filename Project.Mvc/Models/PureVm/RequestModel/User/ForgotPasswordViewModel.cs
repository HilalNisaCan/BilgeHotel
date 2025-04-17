using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Models.PureVm.RequestModel.User
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }
    }
}
