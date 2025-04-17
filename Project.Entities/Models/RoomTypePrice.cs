using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class RoomTypePrice:BaseEntity
    {
        public int Id { get; set; }

        public RoomType RoomType { get; set; }  // Enum

        public decimal PricePerNight { get; set; }  // Gecelik fiyat

        //"Oda fiyatlarını frontend’e sabit yazmak yerine, RoomTypePrice tablosunu oluşturduk. Böylece hem yönetici panelinden güncellenebilir hale geldi, hem de rezervasyon ve ödeme işlemleri her zaman veritabanıyla tutarlı çalışıyor."
    }
}
