using Project.Entities.Enums;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Campaign
{
    public class CampaignResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ReservationPackage Package { get; set; }

        public decimal DiscountPercentage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        // Ekstra gösterim amaçlı
        public string PackageDisplay => Package.ToString();
        public string DateRange => $"{StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}";
        public string Status => IsActive ? "Aktif" : "Pasif";
    }
}
