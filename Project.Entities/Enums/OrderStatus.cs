using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum OrderStatus
    {
        Pending,     // Beklemede
        Preparing,   // Hazırlanıyor
        Delivered,   // Teslim Edildi
        Canceled     // İptal Edildi
    }
}

