using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class ExchangeRateRepository : BaseRepository<ExchangeRate>, IExchangeRateRepository
    {
        public ExchangeRateRepository(MyContext context) : base(context)
        {
        }

        public async Task<ExchangeRate> GetLatestExchangeRateAsync(string currency)
        {
            return await _dbSet.Where(er => er.CurrencyCode == currency)
                               .OrderByDescending(er => er.Date)
                               .FirstOrDefaultAsync();
        }

        public async Task<List<ExchangeRate>> GetExchangeRatesByDateAsync(DateTime date)
        {
            return await _dbSet.Where(er => er.Date.Date == date.Date).ToListAsync();
        }
    }
}
