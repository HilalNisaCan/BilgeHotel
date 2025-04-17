using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{

    /// <summary>
    /// Bu sınıf, müşterinin restoran, bar veya oda servisi gibi hizmetlerden verdiği siparişlerin temel kaydını temsil eder.
    /// Şu an sistemde aktif olarak kullanılmamaktadır, ancak gelecekte detaylı sipariş takibi için altyapı olarak hazır bırakılmıştır.
    /// </summary>
    public class Order : BaseEntity
    {
        // Siparişi veren kullanıcı (müşteri)
        public int UserId { get; set; }

        // Siparişin bağlı olduğu rezervasyon
        public int ReservationId { get; set; }

        // (Opsiyonel) Bu sipariş doğrudan bir ödeme kaydına bağlıysa
        public int? PaymentId { get; set; } // 💡 Eğer ödeme anında alındıysa, burada tutulabilir

        // Siparişin oluşturulma tarihi
        public DateTime OrderDate { get; set; }

        // Siparişin tipi (örn: Oda Servisi, Havuz Başı, Restoran)
        public OrderType OrderType { get; set; } // 💡 Siparişlerin kategorize edilmesi UI ve raporlama için önemlidir

        // Sipariş durumu (Bekliyor, Hazırlanıyor, Teslim Edildi, İptal Edildi)
        public OrderStatus OrderStatus { get; set; }

        // Sipariş toplam tutarı (OrderDetail üzerinden hesaplanmalı)
        public decimal TotalAmount { get; set; }


        //relational properties
        public virtual User User { get; set; } = null!;
        public virtual Reservation Reservation { get; set; } =null!;
        public virtual Payment Payment { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = null!;
    }
}
