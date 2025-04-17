using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IExchangeRateRepository:IRepository<ExchangeRate>
    {
        Task<ExchangeRate> GetLatestExchangeRateAsync(string currency); // Belirli bir döviz birimi için en güncel kuru getir
        Task<List<ExchangeRate>> GetExchangeRatesByDateAsync(DateTime date); // Belirli bir tarihteki döviz kurlarını getir
       
    }
}
