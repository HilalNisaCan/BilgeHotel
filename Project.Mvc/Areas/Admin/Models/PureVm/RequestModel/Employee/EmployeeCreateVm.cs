using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Employee
{
    public class EmployeeCreateVm
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? IdentityNumber { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        [Required]
        public string? PositionName { get; set; }

        [Required]
        public string? SalaryType { get; set; }

        public decimal HourlyWage { get; set; }
        public string? WeeklyOffDay { get; set; }
    }
}
