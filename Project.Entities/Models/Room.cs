using Project.Entities.Enums;
using Project.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class Room:BaseEntity
    {
        // Oda numarası (örnek: 101, 305)
        public string RoomNumber { get; set; } = null!;

        // Odanın bulunduğu kat
        public int FloorNumber { get; set; }

        // Oda tipi: Tek kişilik, Çift kişilik, Süit vb. (Enum)
        public RoomType RoomType { get; set; }

        // Oda Özellikleri
        public bool HasBalcony { get; set; } // 3. ve 4. katta balkon var
        public bool HasMinibar { get; set; } // Tek kişilik odalarda yok
        public bool HasAirConditioning { get; set; } = true; // Klima
        public bool HasTV { get; set; } = true; // Televizyon
        public bool HasHairDryer { get; set; } = true; // Saç kurutma makinesi
        public bool HasWirelessInternet { get; set; } = true; // Kablosuz internet

        // Gecelik fiyat bilgisi
        public decimal PricePerNight { get; set; }

        // Temizlik durumu
        public bool IsCleaned { get; set; }

        // Oda statüsü (Boş, Dolu, Temizleniyor, Bakımda vs.)
        public RoomStatus RoomStatus { get; set; }

        // ✅ Oda kapasitesi: kaç kişi kalabilir
        public int Capacity { get; set; }

        public string? Description { get; set; }


        //relational properties
        public virtual ICollection<RoomMaintenance> RoomMaintenance { get; set; } = null!;
        public virtual ICollection<RoomMaintenanceAssignment> MaintenanceAssignments { get; set; } = null!; // ✅ Oda bakım atamaları
        public virtual ICollection<Reservation> Reservations { get; set; } = null!; // 1-M İlişki
        public virtual ICollection<RoomImage> RoomImages { get; set; } = null!; // Odaya ait fotoğraflar
        public virtual ICollection<RoomCleaningSchedule> CleaningSchedules { get; set; } = null!;
        public virtual ICollection<GuestVisitLog> GuestVisitLogs { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; } = null!;   
    }
}
