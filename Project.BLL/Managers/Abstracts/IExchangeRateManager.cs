using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    /// <summary>
    /// Döviz kuru işlemlerini yöneten manager arayüzüdür.
    /// </summary>
    public interface IExchangeRateManager : IManager<ExchangeRateDto, ExchangeRate>
    {
        /// <summary>
        /// Manuel olarak döviz kuru ekler (ör. Admin tarafından).
        /// </summary>
        Task<bool> AddManualExchangeRatesAsync(List<ExchangeRateDto> rates);

        /// <summary>
        /// API üzerinden en güncel döviz kurlarını çeker (entegre edilirse).
        /// </summary>
        Task<List<ExchangeRateDto>> FetchLatestExchangeRatesAsync(); // TODO: API bağlanırsa entegre edilecek

        /// <summary>
        /// Kur bilgilerine göre para birimi dönüştürür.
        /// </summary>
        Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount, DateTime date);

        /// <summary>
        /// Belirli bir tarihteki döviz kurlarını getirir.
        /// </summary>
        Task<List<ExchangeRateDto>> GetRatesByDateAsync(DateTime date);

        /// <summary>
        /// Belirli bir para birimi için kur günceller veya ekler.
        /// </summary>
        Task<bool> UpdateExchangeRateAsync(ExchangeRateDto dto);
    }
}
