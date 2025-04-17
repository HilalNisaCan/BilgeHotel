using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum MaintenanceStatus
    {
        Pending = 0,      // Beklemede
        Scheduled = 1,    // Planlandı
        Continues = 2,    // Devam Ediyor
        Completed = 3,    // Tamamlandı
        Cancelled = 4     // İptal Edildi

    }
}
