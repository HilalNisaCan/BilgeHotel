using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class PaymentDto:BaseDto
    {
       
        public int? UserId { get; set; } // Ödemeyi yapan kullanıcının ID'si

        [Required]
        public int ReservationId { get; set; } // Ödeme ait olduğu rezervasyonun ID'si

        public int CustomerId { get; set; }

        [Range(0, 100)]
        public decimal ExchangeRate { get; set; } // Kullanılan döviz kuru

        [Range(0, 100000)]
        public decimal TotalAmount { get; set; } // Toplam ödeme miktarı

        [Range(0, 100000)]
        public decimal PaidAmount { get; set; } // Şimdiye kadar ödenen miktar

        public DateTime? PaymentDate { get; set; } // Ödemenin yapıldığı tarih

        public PaymentMethod PaymentMethod { get; set; } // Nakit, Havale, Kredi Kartı

        public PaymentStatus PaymentStatus { get; set; } // Tamamlandı, Beklemede, İptal vb.

        public string? InvoiceNumber { get; set; } // Fatura numarası (opsiyonel)

        public string? Description { get; set; } // Açıklama (opsiyonel)

        public string? TransactionId { get; set; } // Banka işlem numarası (opsiyonel)

        public string? CancellationReason { get; set; } // İptal sebebi (opsiyonel)

        public bool IsAdvancePayment { get; set; } // Avans ödemesi mi?

        public bool IsRefunded { get; set; } // Geri ödeme yapıldı mı?

        public DateTime? LastUpdated { get; set; } // Son güncellenme zamanı
    }
}
