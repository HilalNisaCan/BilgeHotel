using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.ExcahangeRate;

namespace Project.MvcUI.Areas.Admin.Models.PageVm
{
    // 💼 Tüm kur verilerini listeleyecek ViewModel (Index için)
    public class ExchangeRatePageVm
    {
        public List<ExchangeRateResponseModel> ExchangeRates { get; set; } = new();
    }
}
