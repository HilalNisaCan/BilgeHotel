using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class EmployeeShiftDto:BaseDto
    {

        [Required]
        public ShiftType ShiftType { get; set; } // Vardiya tipi (Sabah, Akşam, Gece)

        [Required]
        public DateTime ShiftDate { get; set; } // Vardiyanın uygulanacağı tarih

        [Required]
        public TimeSpan ShiftStart { get; set; } // Başlangıç saati (örn: 08:00)

        [Required]
        public TimeSpan ShiftEnd { get; set; } // Bitiş saati (örn: 16:00)

        public string? Description { get; set; } // Açıklama (nullable yapılmalı)

        public bool IsActive { get; set; } // Aktif mi?

        public bool HasOvertime { get; set; } // Fazla mesai var mı?

        [Range(0, 1000)]
        public decimal OvertimePay { get; set; } // Fazla mesai varsa ödeme miktarı

        public bool IsDayOff { get; set; } // Tatil günü mü?

        // ⬇️ Çalışan atamaları
        public List<EmployeeShiftAssignmentDto> AssignedEmployees { get; set; } = new();

    }
}
