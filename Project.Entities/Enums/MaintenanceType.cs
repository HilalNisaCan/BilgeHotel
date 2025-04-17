using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Enums
{
    public enum MaintenanceType
    {
        DailyCleaning=0,         // Günlük Temizlik
        TechnicalMaintenance=1,  // Teknik Bakım
        AirConditioning=2,       // Klima Bakımı
        ElectricalIssue=3,       // Elektrik Sorunu
        Plumbing=4,              // Sıhhi Tesisat Bakımı
        GeneralMaintenance=5    // Genel Bakım
    }
}
