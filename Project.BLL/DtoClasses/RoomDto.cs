using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class RoomDto:BaseDto
    {
        [Required, StringLength(10)]
        public int RoomNumber { get; set; } // Oda numarası (örnek: 102, 3B)

        public int FloorNumber { get; set; } // Odanın bulunduğu kat

        public List<RoomImageDto> RoomImages { get; set; }

        public RoomType RoomType { get; set; } // Oda tipi (Standart, Deluxe, Suite)

        public bool HasBalcony { get; set; } // Balkon var mı?

        public bool HasMinibar { get; set; } // Minibar var mı?

        public bool HasAirConditioning { get; set; } = true; // Klima var mı?

        public bool HasTV { get; set; } = true; // Televizyon var mı?

        public bool HasHairDryer { get; set; } = true; // Saç kurutma makinesi var mı?

        public bool HasWirelessInternet { get; set; } = true; // Kablosuz internet var mı?
      
        public int Capacity { get; set; }


        [Range(0, 10000)]
        public decimal PricePerNight { get; set; } // Gecelik fiyat

        public bool IsCleaned { get; set; } // Oda temiz mi?

        public string? Description { get; set; }

        public RoomStatus Status { get; set; } // Oda durumu (Boş, Dolu, Temizlikte vs.)
    }
}
