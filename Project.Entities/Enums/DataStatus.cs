using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    // Tabloların aktiflik durumlarını belirleyen enum.
    public enum DataStatus
    {
        Inserted = 0,// Veri aktif olarak kullanılıyor
        Updated =1,  // Veri güncellenmiş
        Deleted=3  // Veri silinmiş (soft delete)
    }
}
