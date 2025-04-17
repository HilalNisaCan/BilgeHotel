using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class EmployeeShiftAssignment:BaseEntity
    {
        // Atama yapılan çalışan
        public int EmployeeId { get; set; }

        // Atanılan vardiya
        public int EmployeeShiftId { get; set; }

        // Atama tarihi (kimin ne zaman atandığını takip için)
        public DateTime AssignedDate { get; set; }

        public string Description { get; set; } = null!;

        // Opsiyonel: Vardiya bu çalışan tarafından tamamlandıysa bitiş zamanı
        public DateTime? CompletedDate { get; set; }

        //  Vardiya durum takibi yapılacaksa enum eklenebilir (örn: Bekliyor, Görevde, Tamamlandı)
        public ShiftStatus? ShiftStatus { get; set; }

        // İlişkiler
        public virtual Employee Employee { get; set; } = null!;
        public virtual EmployeeShift EmployeeShift { get; set; } = null!;
    }
}
