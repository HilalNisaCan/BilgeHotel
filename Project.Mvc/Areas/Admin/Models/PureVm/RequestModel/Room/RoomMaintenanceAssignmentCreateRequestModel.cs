using Project.Entities.Enums;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Room
{
    public class RoomMaintenanceAssignmentCreateRequestModel
    {
        public int RoomId { get; set; }                       // Oda bilgisi
        public MaintenanceType MaintenanceType { get; set; }  // Yeni: Hangi tip bakım yapılacak?
        public int EmployeeId { get; set; }                   // Personel
        public DateTime AssignedDate { get; set; }            // Atama tarihi
        public string? Description { get; set; }              // Açıklama
    }
}
