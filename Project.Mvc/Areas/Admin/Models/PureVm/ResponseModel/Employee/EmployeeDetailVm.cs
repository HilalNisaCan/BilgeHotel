namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Employee
{
    public class EmployeeDetailVm
    {

        public int Id { get; set; }
        public string? FullName { get; set; }          // Ad + Soyad
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? PositionName { get; set; }
        public string? SalaryType { get; set; }
        public decimal? HourlyWage { get; set; }
        public decimal? MonthlySalary { get; set; }
        public int? FixedSalary { get; set; }
        public TimeSpan? ShiftStartTime { get; set; }
        public TimeSpan? ShiftEndTime { get; set; }
        public string? WeeklyOffDay { get; set; }
        public string? Address { get; set; }
    }
}
