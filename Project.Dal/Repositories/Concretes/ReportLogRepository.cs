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

        // Özel log ekleme işlemi — eksik alanları burada tamamlayabiliriz
        public async Task AddLogAsync(ReportLog log)
        {
            // Eğer mesaj boşsa varsayılan veriyoruz
            if (string.IsNullOrWhiteSpace(log.LogMessage))
                log.LogMessage = "Sistem logu";

            // Sistem loglarında müşteri bilgisi yoksa null geçilebilir
            if (log.IsSystemGenerated)
            {
                log.UserId = null;
                log.CustomerId = null;
            }

            log.CreatedDate = DateTime.Now;

            await _dbSet.AddAsync(log);
            await _context.SaveChangesAsync(); // EF üzerinden veritabanına kayıt
        }

       
    }
}
