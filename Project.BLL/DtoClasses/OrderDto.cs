using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class OrderDto:BaseDto
    {
        [Required]
        public int UserId { get; set; } // Siparişi veren kullanıcının ID'si

        [Required]
        public int ReservationId { get; set; } // Siparişin ait olduğu rezervasyonun ID'si

        public DateTime OrderDate { get; set; } // Siparişin verildiği tarih

        public OrderStatus Status { get; set; } // Sipariş durumu (örn: Bekliyor, Teslim Edildi)

        [Range(0, 100000)]
        public decimal TotalAmount { get; set; } // Siparişin toplam tutarı
    }
}
