using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class RoomMaintenance:BaseEntity
    {

        // Hangi odada bakım yapılıyor?
        public int RoomId { get; set; }


        // Bakım tipi: Elektrik, Klima, Sıhhi Tesisat, Genel Bakım gibi
        public MaintenanceType MaintenanceType { get; set; }

        // Planlanan bakım tarihi (henüz başlamamış olabilir)
        public DateTime ScheduledDate { get; set; }

        // Bakımın fiilen başladığı tarih
        public DateTime StartDate { get; set; }

        // Bakımın bittiği tarih (henüz bitmemişse null olabilir)
        public DateTime? EndDate { get; set; }

        // Bakım durumu: Bekliyor, Başladı, Tamamlandı gibi
        public MaintenanceStatus MaintenanceStatus { get; set; }

        // Açıklama veya bakım notları
        public string? Description { get; set; }



        //relational properties

        // Hangi oda?
        public virtual Room Room { get; set; } = null!;

        // Bu bakıma atanmış tüm çalışanlar (çoka çok ilişki)
        public virtual ICollection<RoomMaintenanceAssignment> MaintenanceAssignments { get; set; }=null!;
    }

}

