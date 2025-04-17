using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IBackUpRepository:IRepository<BackupLog>
    {
        Task<BackupLog> GetLatestBackupAsync(); // En son alınan yedeği getir
        Task<List<BackupLog>> GetBackupsByUserIdAsync(int userId); // Belirli bir kullanıcı tarafından alınan yedekleri getir
        Task<List<BackupLog>> GetRestoredBackupsAsync(); // Geri yüklenmiş yedekleri getir
    }
}
