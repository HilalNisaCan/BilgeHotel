using Project.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Campaign
{
    public class CampaignRequestModel
    {
        [Required(ErrorMessage = "Kampanya adı zorunludur.")]
        [Display(Name = "Kampanya Adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Paket türü seçilmelidir.")]
        [Display(Name = "Paket Türü")]
        public ReservationPackage Package { get; set; }

        [Required(ErrorMessage = "İndirim oranı gereklidir.")]
        [Range(1, 100, ErrorMessage = "İndirim oranı 1 ile 100 arasında olmalıdır.")]
        [Display(Name = "İndirim (%)")]
        public decimal DiscountPercentage { get; set; }

        [Required(ErrorMessage = "Başlangıç tarihi girilmelidir.")]
        [DataType(DataType.Date)]
        [Display(Name = "Başlangıç Tarihi")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi girilmelidir.")]
        [DataType(DataType.Date)]
        [Display(Name = "Bitiş Tarihi")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; }
    }
}
