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
    public class GuestVisitLogRepository : BaseRepository<GuestVisitLog>, IGuestVisitLogRepository
    {
        public GuestVisitLogRepository(MyContext context) : base(context)
        {
        }

        // Belirli bir müşterinin ziyaret kayıtlarını getir
        public async Task<List<GuestVisitLog>> GetVisitsByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Where(gvl => gvl.CustomerId == customerId)
                .OrderByDescending(gvl => gvl.EntryDate)
                .ToListAsync();
        }

        // Belirli bir tarihte yapılan ziyaretleri getir
        public async Task<List<GuestVisitLog>> GetVisitsByDateAsync(DateTime date)
        {
            return await _dbSet
                .Where(gvl => gvl.EntryDate.Date == date.Date)
                .ToListAsync();
        }

        // Son X ziyaret kaydını getir
        public async Task<List<GuestVisitLog>> GetRecentVisitsAsync(int count)
        {
            return await _dbSet
                .OrderByDescending(gvl => gvl.EntryDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
