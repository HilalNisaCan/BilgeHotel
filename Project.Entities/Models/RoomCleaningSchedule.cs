using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class RoomCleaningSchedule:BaseEntity
    {
        // Hangi oda temizleniyor?
        public int RoomId { get; set; }

        // Temizlik için planlanan tarih (örnek: günlük, haftalık rutin temizlik planı)
        public DateTime ScheduledDate { get; set; }

        // Temizlik durumu (Bekliyor, Temizleniyor, Tamamlandı gibi)
        public CleaningStatus CleaningStatus { get; set; }

        // Temizlik göreviyle ilgili notlar (örnek: "Banyo ekstra kontrol", "Yatak değiştirilecek")
        public string? Description { get; set; }

        // Görevli temizlik çalışanı
        public int? AssignedEmployeeId { get; set; }

        // Temizlik tamamlandı mı? (Check-out sonrası temizlik gibi işlemler için takip)
        public bool IsCompleted { get; set; }

        //relaitonal properties
        public virtual Room Room { get; set; } = null!; // Temizlik yapılan oda
        public virtual Employee AssignedEmployee { get; set; } = null!;// Görevli personel
    }
}
