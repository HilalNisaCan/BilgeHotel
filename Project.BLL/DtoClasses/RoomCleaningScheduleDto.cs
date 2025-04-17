using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class RoomCleaningScheduleDto:BaseDto
    {
        [Required]
        public int RoomId { get; set; } // Temizlik planlanan odanın ID'si

        [Required]
        public DateTime ScheduledDate { get; set; } // Temizlik yapılacak tarih

        public CleaningStatus CleaningStatus { get; set; } // Temizlik durumu (Planlandı, Yapıldı, vb.)

        public string? Description { get; set; } // Ek açıklama (opsiyonel)

        public int AssignedEmployeeId { get; set; } // Temizlikten sorumlu personel ID'si

        public RoomStatus Status { get; set; } // Odanın temizlik sonrası durumu

        public bool IsCompleted { get; set; } // Temizlik tamamlandı mı?
    }
}
