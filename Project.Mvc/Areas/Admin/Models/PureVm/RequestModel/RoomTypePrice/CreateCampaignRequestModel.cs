using Project.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.RoomTypePrice
{
    public class CreateCampaignRequestModel
    {
        [Required(ErrorMessage = "Paket seçimi zorunludur.")]
        public ReservationPackage Package { get; set; }

        [Required(ErrorMessage = "İndirim oranı zorunludur.")]
        [Range(0.01, 1.0, ErrorMessage = "İndirim oranı 0.01 ile 1.0 arasında olmalıdır.")]
        public decimal DiscountRate { get; set; }

        [Required(ErrorMessage = "Minimum gün sayısı girilmelidir.")]
        [Range(1, 365, ErrorMessage = "1 ile 365 gün arasında olmalıdır.")]
        public int MinDayThreshold { get; set; }
    }
}
