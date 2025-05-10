using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Models.PureVm.RequestModel.Contact
{
    public class ContactFormRequestModel
    {
        [Required(ErrorMessage = "Adınız gereklidir.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konu boş olamaz.")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mesaj alanı boş olamaz.")]
        [StringLength(1000, ErrorMessage = "Mesaj 1000 karakteri geçemez.")]
        public string Message { get; set; } = string.Empty;
    }
}
