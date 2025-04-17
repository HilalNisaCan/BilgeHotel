using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.EmployeeShift
{
    public class EmployeeShiftUpdateVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlangıç saati zorunludur")]
        public TimeSpan ShiftStart { get; set; }

        [Required(ErrorMessage = "Bitiş saati zorunludur")]
        public TimeSpan ShiftEnd { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ShiftDate { get; set; }

        public bool IsDayOff { get; set; } // Vardiya günü izin mi?
        public bool HasOvertime { get; set; } // Fazla mesai var mı?

        [Range(0, 1000, ErrorMessage = "Mesai ücreti 0 ile 1000 arasında olmalı")]
        public decimal OvertimePay { get; set; } = 0;
    }
}
