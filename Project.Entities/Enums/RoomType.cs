using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum RoomType
    {
        Single = 0,              // Tek Kişilik
        DoubleBed = 1,           // İki Kişilik (Tek Yatak)
        TwinBed = 2,             // İki Kişilik (Çift Yatak / Duble)
        Triple = 3,              // Üç Kişilik (Tek + Duble Yatak)
        Quad = 4,                // Dört Kişilik
        KingSuite = 5            // Kral Dairesi
    }
}
