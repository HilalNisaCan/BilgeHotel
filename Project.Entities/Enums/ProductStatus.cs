using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum ProductStatus
    {
        Active = 1,     // Ürün aktif ve satışta
        Passive = 2,    // Ürün geçici olarak satış dışı
        Deleted = 3     // Ürün sistemden kaldırıldı
    }
}
