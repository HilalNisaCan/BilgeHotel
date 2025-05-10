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
        /// ID'ye göre döviz kuru kaydını siler.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            ExchangeRate entity = new ExchangeRate { Id = id }; // ID ile silme işlemi
            await _exchangeRateRepository.RemoveAsync(entity);   // Doğrudan sil
            return true;
        }





    }
}
