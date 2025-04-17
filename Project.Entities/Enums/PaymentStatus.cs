using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum PaymentStatus
    {
        Completed=0,   // Ödeme tamamlandı
        Pending=1,     // Ödeme beklemede
        Cancelled=2,   // Ödeme iptal edildi
        Refunded=3     // Ödeme iade edildi
    }
}
