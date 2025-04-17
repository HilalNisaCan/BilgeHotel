namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.EmployeeShift
{
    public class EmployeeShiftAssignmentResponseVm
    {
        public int Id { get; set; }
        public string EmployeeFullName { get; set; }
        public string ShiftType { get; set; }
        public string ShiftHours { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string ShiftStatus { get; set; }
    }
}
