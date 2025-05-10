using Project.Entities.Enums;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.ExcahangeRate
{
    // 📊 Index.cshtml sayfasında görüntülenecek liste modeli
    public class ExchangeRateResponseModel
    {
        public int Id { get; set; }

        public string CurrencyCode { get; set; } = string.Empty;

        public Currency FromCurrency { get; set; }

        public Currency ToCurrency { get; set; }

        public decimal Rate { get; set; }

        public DateTime Date { get; set; }
    }
}
