using System.ComponentModel.DataAnnotations;
using Project.Entities.Enums;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.ExcahangeRate
{
    // 📝 Yeni kur ekleme formunda kullanılacak model
    public class ExchangeRateRequestModel
    {
        [Required(ErrorMessage = "Para birimi kodu zorunludur.")]
        [StringLength(5)]
        public string CurrencyCode { get; set; } = string.Empty;

        [Required]
        public Currency FromCurrency { get; set; }

        [Required]
        public Currency ToCurrency { get; set; }

        [Range(0.0, 100.0)]
        public decimal Rate { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
