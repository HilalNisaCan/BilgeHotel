using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    /// <summary>
    /// Bu sınıf, bir siparişin içerisindeki ürün kalemlerini ve miktarlarını tutmak için tasarlanmıştır.
    /// Her Order kaydı için birden fazla OrderDetail olabilir. Şu an sistemde aktif değildir.
    /// Gelecekte ürün bazlı faturalama veya detaylı harcama takibi için kullanılabilir.
    /// </summary>
    /// 
    // NOT: Bu yapı şu anda sistemde kullanılmamaktadır.
    // Eğer ileride otel içinde yemek, içecek, oda servisi gibi sipariş modülü geliştirilirse aktif hale getirilebilir.
    public class OrderDetail : BaseEntity
    {
        // Bu detay hangi siparişe ait?
        public int OrderId { get; set; }

        // Hangi ürün sipariş edildi?
        public int ProductId { get; set; }

        // Kaç adet sipariş edildi?
        public int Quantity { get; set; }

        // Sipariş sırasında ürünün birim fiyatı (değişebilir diye sabitlenmeli)
        public decimal UnitPrice { get; set; }

        //relational properties
        public virtual Order Order { get; set; } = null!; // Sipariş bilgisi
        public virtual Product Product { get; set; } = null!; // Ürün bilgisi
      

    }
}
