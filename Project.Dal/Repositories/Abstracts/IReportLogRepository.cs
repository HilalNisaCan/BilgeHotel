using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IReportLogRepository:IRepository<ReportLog>
    {
        Task<List<ReportLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<ReportLog>> GetLogsByUserIdAsync(int userId);
        Task<List<ReportLog>> GetLogsByIPAddressAsync(string ipAddress);
    }
}

