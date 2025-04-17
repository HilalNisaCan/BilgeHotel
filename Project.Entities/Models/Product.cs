using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{   
    public class Product:BaseEntity
    {
        // Ürün adı (örnek: "Kola", "Çikolatalı Gofret")
        public string? Name { get; set; }

        // Ürün fiyatı (örnek: 35.50₺)
        public decimal Price { get; set; }

        // Ürün kategorisi: Minibar, Bar, Oda Servisi vb. (Enum)
        public ProductCategory Category { get; set; }

        public ProductStatus ProductStatus { get; set; }

        // Ürün stokta var mı? (bittiğinde sipariş edilemesin)
        public bool IsInStock { get; set; }

        // Ürün açıklaması (örnek: "Şekersiz kola", "Glutensiz çikolata")
        public string? Description { get; set; }

        // Ürün görsel yolu (sunucu içi dosya veya URL olabilir)
        public string? ImagePath { get; set; }

        //relational properties
        public virtual ICollection<ExtraExpense> ExtraExpenses { get; set; } = null!; // Harcama kayıtları
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = null!;  // Sipariş detayları

    }
}
