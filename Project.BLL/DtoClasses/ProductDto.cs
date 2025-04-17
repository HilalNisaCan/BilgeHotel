using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class ProductDto:BaseDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; } // Ürün adı

        [Range(0, 100000)]
        public decimal Price { get; set; } // Ürün fiyatı

        [Required]
        public ProductCategory Category { get; set; } // Ürün kategorisi

        public bool IsInStock { get; set; } // Ürün stokta var mı?

        public string Description { get; set; } // Ürün açıklaması (opsiyonel)

        public string ImagePath { get; set; } // Ürün görsel yolu (opsiyonel)
    }
}
