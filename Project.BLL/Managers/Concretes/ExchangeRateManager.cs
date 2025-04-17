using AutoMapper;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    /// <summary>
    /// Döviz kuru işlemlerini gerçekleştiren manager sınıfı.
    /// </summary>
    public class ExchangeRateManager : BaseManager<ExchangeRateDto, ExchangeRate>, IExchangeRateManager
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IMapper _mapper;

        public ExchangeRateManager(IExchangeRateRepository exchangeRateRepository, IMapper mapper)
            : base(exchangeRateRepository, mapper)
        {
            _exchangeRateRepository = exchangeRateRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Manuel döviz kuru ekler (aynı tarihte varsa günceller).
        /// </summary>
        public async Task<bool> AddManualExchangeRatesAsync(List<ExchangeRateDto> rates)
        {
            foreach (var dto in rates)
            {
                var existing = await _exchangeRateRepository
                    .FirstOrDefaultAsync(x => x.CurrencyCode == dto.CurrencyCode && x.Date.Date == dto.Date.Date);

                if (existing != null)
                {
                    existing.Rate = dto.Rate;
                    await _exchangeRateRepository.UpdateAsync(existing);
                }
                else
                {
                    var entity = _mapper.Map<ExchangeRate>(dto);
                    await _exchangeRateRepository.AddAsync(entity);
                }
            }

            return true;
        }

        /// <summary>
        /// API'den döviz kuru alımı (şu an pasif - dış servis bekleniyor).
        /// </summary>
        public async Task<List<ExchangeRateDto>> FetchLatestExchangeRatesAsync()
        {
            // TODO: API bağlantısı yapılırsa buraya entegre edilecek
            return new List<ExchangeRateDto>();
        }

        /// <summary>
        /// İki para birimi arasında belirli bir tarihte dönüşüm yapar.
        /// </summary>
        public async Task<decimal> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount, DateTime date)
        {
            var rates = await _exchangeRateRepository.GetAllAsync(x => x.Date.Date == date.Date);
            var fromRate = rates.FirstOrDefault(x => x.CurrencyCode == fromCurrency)?.Rate ?? 1;
            var toRate = rates.FirstOrDefault(x => x.CurrencyCode == toCurrency)?.Rate ?? 1;

            if (fromRate == 0 || toRate == 0)
                throw new Exception("Döviz kuru bilgileri eksik.");

            return (amount / fromRate) * toRate;
        }

        /// <summary>
        /// Belirli bir tarihe ait döviz kurlarını listeler.
        /// </summary>
        public async Task<List<ExchangeRateDto>> GetRatesByDateAsync(DateTime date)
        {
            var rates = await _exchangeRateRepository.GetAllAsync(x => x.Date.Date == date.Date);
            return _mapper.Map<List<ExchangeRateDto>>(rates);
        }

        /// <summary>
        /// Var olan döviz kurunu günceller, yoksa yeni ekler.
        /// </summary>
        public async Task<bool> UpdateExchangeRateAsync(ExchangeRateDto dto)
        {
            var existing = await _exchangeRateRepository
                .FirstOrDefaultAsync(x => x.CurrencyCode == dto.CurrencyCode && x.Date.Date == dto.Date.Date);

            if (existing != null)
            {
                existing.Rate = dto.Rate;
                await _exchangeRateRepository.UpdateAsync(existing);
                return true;
            }
            else
            {
                var entity = _mapper.Map<ExchangeRate>(dto);
                await _exchangeRateRepository.AddAsync(entity);
                return true;
            }
        }


    }
}
