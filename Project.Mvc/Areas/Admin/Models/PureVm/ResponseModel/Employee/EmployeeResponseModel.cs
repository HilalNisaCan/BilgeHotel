using Project.Entities.Enums;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Employee
{
    public class EmployeeResponseModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName => $"{FirstName} {LastName}";

        public string Position { get; set; } = null!;          // View: @employee.Position
        public string PositionName { get; set; } = null!;      // Eski kullanım kalabilir
        public string Phone { get; set; } = null!;             // View: @employee.Phone
        public string PhoneNumber { get; set; } = null!;       // Eski kullanım
        public string Email { get; set; } = null!;
        public string IdentityNumber { get; set; } = null!;
        public string Address { get; set; } = null!;

        public string SalaryType { get; set; } = null!;
        public decimal? HourlyWage { get; set; }               // View: HasValue için nullable
        public string HourlyWageFormatted { get; set; } = "-"; // ₺/saat formatı

        public string ShiftInfo { get; set; } = "-";           // View: @employee.ShiftInfo
        public string ShiftTime { get; set; } = "-";           // Alternatif
        public string DayOff { get; set; } = "-";              // View: @employee.DayOff
        public string WeeklyOffDay { get; set; } = "-";        // Alternatif

        public string HireDate { get; set; } = "-";            // 01.01.2023 gibi gösterim
        public bool HasOvertime { get; set; }
        public bool IsActive { get; set; }
    }
}
