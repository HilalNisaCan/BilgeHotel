namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Employee
{
    public class UpdateEmployeeRequest
    {
        public int Id { get; set; } // Güncellenecek çalışan Id’si

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Position { get; set; } // Enum string (örnek: "Receptionist", "Chef")

        public string? SalaryType { get; set; } // Enum string: Hourly, Monthly
        public decimal? HourlyWage { get; set; } // Saatlik ücret sadece saatlikler için geçerli
        public decimal? MonthlySalary { get; set; } // Aylık maaşlı çalışanlar için
        public string? IdentityNumber { get; set; }
        public string? Address { get; set; }

        public string? DayOff { get; set; } // Haftalık izin günü

        public bool IsActive { get; set; }

        // Varsayılan Vardiya
        public string? ShiftStart { get; set; } // örn: "08:00"
        public string? ShiftEnd { get; set; }   // örn: "16:00"

        public bool HasExtraShift { get; set; } // Ek mesai varsa
    }
}
