using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum DiscountType
    {
        None,      // ✅ İndirim yok
        OneMonth,  // ✅ 1 ay öncesine kadar yapılan rezervasyonlar
        ThreeMonths // ✅ 3 ay öncesine kadar yapılan rezervasyonlar
    }
}
