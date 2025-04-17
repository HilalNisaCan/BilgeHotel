using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DtoClasses
{
    public class ExchangeRateDto:BaseDto
    {
        [Required]
        [StringLength(5)]
        public string CurrencyCode { get; set; } = null!;// Para biriminin kodu (örn: USD, EUR)

        [Required]
        public Currency FromCurrency { get; set; } // Kaynak para birimi (örn: USD)

        [Required]
        public Currency ToCurrency { get; set; } // Hedef para birimi (örn: TRY)

        [Range(0.0, 100.0)]
        public decimal Rate { get; set; } // İlgili kur oranı

        public DateTime Date { get; set; } // Kurun geçerli olduğu tarih
    }
}
