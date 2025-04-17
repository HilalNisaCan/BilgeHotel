using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum GuestVisitStatus
    {
        CheckedIn = 0,       // Müşteri odaya giriş yaptı
        CheckedOut = 1,      // Müşteri odadan çıkış yaptı
        InProgress = 2,      // Misafir odada, süreç devam ediyor
        Completed = 3        // Misafir ziyaretini tamamladı
    }
}
