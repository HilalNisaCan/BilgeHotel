using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum ReservationStatus
    {
        Waiting = 0,     // Onay bekliyor
        Confirmed = 1,   // Onaylandı
        Cancelled = 2,   // İptal edildi
        Completed = 3 // ✅ Çıkış yapılan
    }
}
