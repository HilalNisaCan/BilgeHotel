using Project.BLL.DtoClasses;

namespace Project.MvcUI.Areas.Admin.Models.PageVm
{
    public class CustomerReportPageVm
    {

        public int Id { get; set; }
        public string FullName { get; set; }
        public string IdentityNumber { get; set; }
        public string PhoneNumber { get; set; }
        public int TotalReservationCount { get; set; }
        public decimal TotalSpent { get; set; }
        public int LoyaltyPoints { get; set; }
        public int CampaignUsageCount { get; set; }
        public DateTime? LastReservationDate { get; set; }
        public List<ReservationDto> PastReservations { get; set; } = new();
        public List<ReservationDto> UpcomingReservations { get; set; } = new();
        public List<ReservationDto> CurrentStays { get; set; } = new();
    }
}
