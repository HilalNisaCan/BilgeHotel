using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.EmployeeShift
{
    public class EmployeeShiftAssignmentCreateVm
    {

        [Required(ErrorMessage = "Çalışan seçimi zorunludur.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Vardiya seçimi zorunludur.")]
        public int EmployeeShiftId { get; set; }

        [Required(ErrorMessage = "Tarih seçimi zorunludur.")]
        [DataType(DataType.Date)] // 🟢 Bu, date input'u düzgün bağlaması için önemli
        public DateTime AssignedDate { get; set; }

        public string? Description { get; set; }
    }
}
