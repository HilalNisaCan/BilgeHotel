using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class ExtraExpense:BaseEntity
    {

        // Hangi rezervasyon kapsamında bu harcama yapıldı?
        public int ReservationId { get; set; }

        // Harcamayı yapan müşteri (örnek: ziyaretçi misafir veya rezervasyon sahibi)
        public int CustomerId { get; set; }

        // Harcamanın yapıldığı ürün (Minibar, Restoran, Bar ürünleri)
        public int ProductId { get; set; }

        // Hangi ödeme kaydına bağlı (toplu ödeme yapıldıysa)
        public int? PaymentId { get; set; }

        // Açıklama: "Minibardan 2 kola alındı" gibi
        public string? Description { get; set; }

        // Ürün miktarı
        public int Quantity { get; set; }

        // Ürünün birim fiyatı (Product tablosundaki fiyat değişebilir diye burada da saklanır)
        public decimal UnitPrice { get; set; }

        // Harcamanın yapıldığı tarih
        public DateTime ExpenseDate { get; set; }


        //relational properties
        public virtual Customer Customer { get; set; } = null!; // Harcamayı yapan müşteri bilgisi

        public virtual Reservation Reservation { get; set; } = null!; // Harcamanın yapıldığı rezervasyon

        public virtual Product? Product { get; set; } // Harcanan ürün bilgisi
      
        public virtual Payment Payment { get; set; } = null!;
    }
}
