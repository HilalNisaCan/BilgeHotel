using Project.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Entities.Models
{
    public class ExchangeRate:BaseEntity
    {
        // Döviz kodu (örn: USD, EUR, GBP)
        public string CurrencyCode { get; set; } = null!;

        // Kaynak para birimi (örn: USD)
        public Currency FromCurrency { get; set; }

        // Hedef para birimi (örn: TRY)
        public Currency ToCurrency { get; set; }

        // Döviz kuru (örnek: 1 USD = 27.80 TRY)
        public decimal Rate { get; set; }

        // Kurun geçerli olduğu tarih
        public DateTime Date { get; set; }

    }
}
