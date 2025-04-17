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
    public class BackupLogRepository : BaseRepository<BackupLog>, IBackUpRepository
    {
        public BackupLogRepository(MyContext context) : base(context)
        {
        }

        // En son alınan yedeği getir
        public async Task<BackupLog> GetLatestBackupAsync()
        {
            return await _dbSet
                .OrderByDescending(b => b.BackupDate)
                .FirstOrDefaultAsync();
        }

        // Belirli bir kullanıcının aldığı yedekleri getir
        public async Task<List<BackupLog>> GetBackupsByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BackupDate)
                .ToListAsync();
        }

        // Geri yüklenmiş yedekleri getir
        public async Task<List<BackupLog>> GetRestoredBackupsAsync()
        {
            return await _dbSet
                .Where(b => b.IsRestored)
                .OrderByDescending(b => b.BackupDate)
                .ToListAsync();
        }
    }
}
