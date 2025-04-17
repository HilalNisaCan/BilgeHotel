using Project.Entities.Enums;
using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class Employee : BaseEntity, IIdentifiablePerson
    {

        // Sisteme giriş yapabilecekse: kullanıcı hesabı FK'sı
        public int? UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; } // Çalışanın telefon numarası
        public string? Address { get; set; } // DİKKAT: önceki hali "Adress" yazılmıştı — düzeltildi
        public string? IdentityNumber { get; set; } // T.C. Kimlik numarası

        // Pozisyon ve Ücret
        public EmployeePosition Position { get; set; } // Yönetici, Resepsiyonist vb.
        public SalaryType SalaryType { get; set; } // Aylık, Saatlik, Prim bazlı
        public decimal HourlyWage { get; set; } = 0;           // Saatlik ücret (0 ise kullanılmaz)
        public decimal MonthlySalary { get; set; } = 0;        // Sabit maaş (yöneticiler için)


        // Vardiya Bilgileri
        public TimeSpan ShiftStart { get; set; } // Vardiya başlangıç saati
        public TimeSpan ShiftEnd { get; set; } // Vardiya bitiş saati
        public string? WeeklyOffDay { get; set; }

        // Mesai ve Çalışma Süresi
        public bool HasOvertime { get; set; } // Fazla mesai yapıldı mı?
        public int WeeklyWorkedHours { get; set; } // Haftalık toplam çalışma süresi
        public int TotalWorkedHours { get; set; } // Toplam çalışma süresi (vardiya takibinden hesaplanır)

        // Çalışan Aktif mi?
        public bool IsActive { get; set; }

        // İşe giriş tarihi
        public DateTime HireDate { get; set; }



        //relational properties
        public virtual User User { get; set; } = null!; // Çalışanın kullanıcı bilgisi

        public virtual ICollection<RoomMaintenanceAssignment> MaintenanceAssignments { get; set; } = null!;// Çalışanın bakım işleri (Çoka çok ilişki)

        public virtual ICollection<RoomCleaningSchedule> RoomCleaningSchedules { get; set; } = null!; // Oda temizliği ilişkisi

        public virtual ICollection<EmployeeShiftAssignment> EmployeeShiftAssignments { get; set; } = null!;// Çalışanın vardiyaları

    }   
}



