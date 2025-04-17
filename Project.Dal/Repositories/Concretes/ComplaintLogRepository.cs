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
    public class ComplaintLogRepository : BaseRepository<ComplaintLog>, IComplaintLogRepository
    {
        public ComplaintLogRepository(MyContext context) : base(context)
        {
        }

        // Çözümlenmemiş şikayetleri getir
        public async Task<List<ComplaintLog>> GetUnresolvedComplaintsAsync()
        {
            return await _dbSet
                .Where(cl => !cl.IsResolved)
                .ToListAsync();
        }

        // Çözümlenmiş şikayetleri getir
        public async Task<List<ComplaintLog>> GetResolvedComplaintsAsync()
        {
            return await _dbSet
                .Where(cl => cl.IsResolved)
                .ToListAsync();
        }

        // Belirli bir müşteriye ait tüm şikayetleri getir
        public async Task<List<ComplaintLog>> GetComplaintsByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Where(cl => cl.CustomerId == customerId)
                .ToListAsync();
        }

        // Belirli bir müşterinin en son yaptığı şikayeti getir
        public async Task<ComplaintLog> GetLatestComplaintByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Where(cl => cl.CustomerId == customerId)
                .OrderByDescending(cl => cl.SubmittedDate)
                .FirstOrDefaultAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var complaint = await GetByIdAsync(id);
            if (complaint != null)
            {
                await base.RemoveAsync(complaint); // ✅ `id` yerine `ComplaintLog` nesnesi gönderilmeli
            }
        }
    }
}
