using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class RoomMaintenanceAssignment:BaseEntity
    {
        // Bakım yapılacak oda
        public int RoomId { get; set; }

        // İlgili bakım kaydı
        public int RoomMaintenanceId { get; set; }

        // Görevli personel
        public int EmployeeId { get; set; } // FK

        // Görevin atandığı tarih
        public DateTime AssignedDate { get; set; }

        // Bakım tamamlandıysa bitiş zamanı (opsiyonel)
        public DateTime? CompletedDate { get; set; }

        // Bu görev için durum (Başladı, Tamamlandı, Bekliyor)
        public MaintenanceStatus? MaintenanceStatus { get; set; }

        public string? Description { get; set; }

        // İlişkisel özellikler
        public virtual Room Room { get; set; } = null!;
        public virtual RoomMaintenance RoomMaintenance { get; set; } = null!;
        public virtual Employee Employee { get; set; } =null!;
    }
}
