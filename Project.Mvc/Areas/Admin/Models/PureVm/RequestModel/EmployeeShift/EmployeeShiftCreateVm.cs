using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.EmployeeShift
{
    public class EmployeeShiftCreateVm
    {
        public TimeSpan ShiftStart { get; set; }
        public TimeSpan ShiftEnd { get; set; }
        public string ShiftType { get; set; } // Sabah, Akşam, Gece vb.
        public string Description { get; set; }  // Vardiya açıklaması
        public bool IsActive { get; set; }  // Vardiya aktif mi?
        [Required(ErrorMessage = "Vardiya tarihi seçilmelidir.")]
        public DateTime ShiftDate { get; set; }

    }
}
