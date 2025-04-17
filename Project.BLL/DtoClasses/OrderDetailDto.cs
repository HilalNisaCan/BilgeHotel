using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class OrderDetailDto:BaseDto
    {

        [Required]
        public int OrderId { get; set; } // Siparişin ID'si

        [Required]
        public int ProductId { get; set; } // Siparişte yer alan ürünün ID'si

        public int ReservationId { get; set; } // Ürünün ait olduğu rezervasyon (isteğe bağlı kullanılabilir)

        [Range(1, 100)]
        public int Quantity { get; set; } // Ürün adedi

        [Range(0, 10000)]
        public decimal UnitPrice { get; set; } // Ürünün birim fiyatı
    }
}
