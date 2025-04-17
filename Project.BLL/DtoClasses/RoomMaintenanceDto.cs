using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class RoomMaintenanceDto:BaseDto
    {


        [Required]
        public int RoomId { get; set; } // Bakım yapılan oda ID'si

        [Required]
        public MaintenanceType MaintenanceType { get; set; } // Bakım türü (Elektrik, Tesisat vb.)

        [Required]
        public DateTime ScheduledDate { get; set; } // Planlanan bakım tarihi

        [Required]
        public DateTime StartDate { get; set; } // Başlangıç tarihi

        public DateTime? EndDate { get; set; } // Bitiş tarihi (opsiyonel)

        public MaintenanceStatus Status { get; set; } // Bakım durumu (Planlandı, Devam Ediyor, Tamamlandı)

        public string? Description { get; set; } // Açıklama (opsiyonel)
    }
}
