using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class EmployeeShiftAssignmentDto:BaseDto
    {
        [Required(ErrorMessage = "Çalışan seçimi zorunludur.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Vardiya seçimi zorunludur.")]
        public int EmployeeShiftId { get; set; }

        [Required(ErrorMessage = "Atama tarihi gereklidir.")]
        public DateTime AssignedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public ShiftStatus? ShiftStatus { get; set; }

        public string? Description { get; set; } // ❗ null olabileceği için nullable

        // Mapping için gerekli navigation property'ler
        public EmployeeDto? Employee { get; set; } // ❗ AutoMapper için nullable
        public EmployeeShiftDto? EmployeeShift { get; set; }
    }
}
