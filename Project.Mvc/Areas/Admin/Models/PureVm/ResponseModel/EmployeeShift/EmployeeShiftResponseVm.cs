namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.EmployeeShift
{
    public class EmployeeShiftResponseVm
    {
        public int Id { get; set; }

        public string? ShiftType { get; set; }

        public DateTime ShiftDate { get; set; }

        public string? ShiftHours { get; set; } // Örn: "08:00 - 16:00"

        public bool HasOvertime { get; set; }

        // Yeni Eklenenler:
        public TimeSpan ShiftStart { get; set; }

        public TimeSpan ShiftEnd { get; set; }

        public double TotalHours => (ShiftEnd - ShiftStart).TotalHours;

        public List<string>? AssignedEmployeesFullNames { get; set; }
        public string? AssignedEmployeeNames { get; set; }
    }
}
