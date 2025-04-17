using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class EmployeeShift:BaseEntity
    {
        // Vardiya tipi (örn: Sabah, Gece, Hafta Sonu) — enum kullanımı esneklik sağlar
        public ShiftType ShiftType { get; set; }

        // Vardiyanın uygulandığı tarih
        public DateTime ShiftDate { get; set; }

        // Başlangıç ve bitiş saatleri
        public TimeSpan ShiftStart { get; set; } // Örn: 08:00
        public TimeSpan ShiftEnd { get; set; }   // Örn: 16:00

        // Fazla mesai var mı?
        public bool HasOvertime { get; set; }

        // Eğer fazla mesai yapıldıysa, ekstra ödeme miktarı
        public decimal OvertimePay { get; set; }

        // Tatil günü mü? (örn: resmi tatil, izin günü)
        public bool IsDayOff { get; set; }

        public string? Description { get; set; }

        //relational properties

        public virtual ICollection<EmployeeShiftAssignment> ShiftAssignments { get; set; } = null!; // Vardiyaya atanan çalışanlar
    }
}
