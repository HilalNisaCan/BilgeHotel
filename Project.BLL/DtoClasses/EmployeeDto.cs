using Project.Entities.Enums;
using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class EmployeeDto:BaseDto
    {
        public int? UserId { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required, StringLength(50)]
        public string LastName { get; set; } = null!;

        public string FullName => $"{FirstName} {LastName}";

        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [StringLength(200)]
        public string? Address { get; set; }

        [Required, StringLength(11, MinimumLength = 11)]
        public string IdentityNumber { get; set; } = null!;

        public EmployeePosition Position { get; set; }

        public SalaryType SalaryType { get; set; }

        [Range(0, 1000)]
        public decimal HourlyWage { get; set; }

        public decimal? MonthlySalary { get; set; } // ✅ EKLENDİ ✨

        public TimeSpan ShiftStart { get; set; }

        public TimeSpan ShiftEnd { get; set; }

        public bool HasOvertime { get; set; }

        public int WeeklyWorkedHours { get; set; }

        public int TotalWorkedHours { get; set; }

        public bool IsActive { get; set; }

        public DateTime? HireDate { get; set; }

        public string? Email { get; set; }

        public string? WeeklyOffDay { get; set; }
    }
}
