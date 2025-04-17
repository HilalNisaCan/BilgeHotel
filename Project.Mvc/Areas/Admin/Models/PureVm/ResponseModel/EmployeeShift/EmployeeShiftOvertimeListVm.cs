namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.EmployeeShift
{
    public class EmployeeShiftOvertimeListVm
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ShiftTime { get; set; }
        public DateTime AssignedDate { get; set; }
        public double WorkedHours { get; set; }
        public double OvertimeHours { get; set; }
    }
}
