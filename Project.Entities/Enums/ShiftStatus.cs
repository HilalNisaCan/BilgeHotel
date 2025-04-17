using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum ShiftStatus
    {
        Assigned = 0,     // Atandı ama henüz başlamadı
        InProgress = 1,   // Şu anda görevde
        Completed = 2,    // Görev başarıyla tamamlandı
        Absent = 3        // Göreve gelmedi / devamsız
    }
}
