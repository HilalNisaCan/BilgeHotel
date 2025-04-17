using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class Payment:BaseEntity
    {
        // Ödemeyi yapan kullanıcı (müşteri)
        public int? UserId { get; set; }

        public int? CustomerId { get; set; }


        // Bu ödeme hangi rezervasyona ait?

        public int ReservationId { get; set; }

        // Ödeme sırasında geçerli olan kur
        public decimal ExchangeRate { get; set; }
      
        // Fiyat bilgiler
        public decimal TotalAmount { get; set; }    // Toplam ücret

        public decimal PaidAmount { get; set; }     // Şu ana kadar ödenen kısım

        // Ödeme zamanı
        public DateTime PaymentDate { get; set; }

        // Ödeme yöntemi (Kredi Kartı, Havale, Nakit, vs.)
        public PaymentMethod PaymentMethod { get; set; }

        // Ödeme durumu (Tamamlandı, Beklemede, İptal Edildi)
        public PaymentStatus PaymentStatus { get; set; }

        // Fatura ve işlem bilgileri
        public string? InvoiceNumber { get; set; }         // Fatura numarası (opsiyonel)
        public string? Description { get; set; }           // Açıklama (opsiyonel)
        public string? TransactionId { get; set; }         // Banka işlem ID'si (opsiyonel)

        // İptal ve iade bilgileri
        public string? CancellationReason { get; set; }    // Ödeme iptal edildiğinde sebep girilir
        public bool IsAdvancePayment { get; set; }        // Ön ödeme mi?
        public bool IsRefunded { get; set; }              // Geri ödeme yapıldı mı?

        // Son güncelleme tarihi
        public DateTime LastUpdated { get; set; }

        //relational properties


        public virtual User? User { get; set; }  // Ödemeyi yapan kullanıcı bilgisi

        public virtual Reservation Reservation { get; set; } = null!; // Ödeme ile ilişkili rezervasyon

        public virtual Customer Customer { get; set; } = null!;

        public virtual ICollection<ExtraExpense> ExtraExpenses { get; set; } = null!;// Ekstra harcamalarla ilişkili

        public virtual ICollection<Order> Orders { get; set; } = null!;           

    }





}

