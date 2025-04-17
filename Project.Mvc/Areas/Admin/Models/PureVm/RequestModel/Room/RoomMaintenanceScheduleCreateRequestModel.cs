namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Room
{
    public class RoomMaintenanceScheduleCreateRequestModel
    {
        public int RoomId { get; set; }

        public int AssignedEmployeeId { get; set; }
        
        public DateTime ScheduledDate { get; set; }

        public string? Description { get; set; }
    }
}
