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
    public class ReportLogRepository : BaseRepository<ReportLog>, IReportLogRepository
    {
        public ReportLogRepository(MyContext context) : base(context) { }

        public async Task<List<ReportLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(rl => rl.CreatedDate >= startDate && rl.CreatedDate <= endDate).ToListAsync();
        }

        public async Task<List<ReportLog>> GetLogsByUserIdAsync(int userId)
        {
            return await _dbSet.Where(rl => rl.UserId == userId).ToListAsync();
        }

        public async Task<List<ReportLog>> GetLogsByIPAddressAsync(string ipAddress)
        {
            return await _dbSet.Where(rl => rl.IPAddress == ipAddress).ToListAsync();
        }
    }
}
