﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum ShiftType
    {
        Morning = 1,  // Sabah 08:00 - 16:00
        Evening = 2,  // Akşam 16:00 - 00:00
        Night = 3,    // Gece 00:00 - 08:00
        Daytime = 4,  // Özel Mesai (Elektrikçi, Bilgi İşlem vb. için 08:00 - 18:00)
        Overtime = 5,  // Ek Mesai
        DayOff=6 // İzin Günü
    }
}
