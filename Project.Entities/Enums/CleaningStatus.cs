using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum CleaningStatus
    {
        Scheduled=0,     // Temizlik planlandı
        InProgress=1,    // Temizlik yapılıyor
        Completed=2     // Temizlik bitti
    }
}
