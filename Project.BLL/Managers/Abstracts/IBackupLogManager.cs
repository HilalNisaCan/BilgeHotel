using Project.BLL.DtoClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    /// <summary>
    /// Veritabanı yedekleme ve geri yükleme işlemlerini tanımlar.
    /// (Simülasyon amaçlı gösterim)
    /// </summary>
    public interface IBackupLogManager : IManager<BackupLogDto, BackupLog>
    {
        /// <summary>
        /// Yedekleme işlemi başlatır (gösterim amaçlı).
        /// </summary>
        Task<bool> BackupDatabaseAsync(int userId, string backupFolderPath);

        /// <summary>
        /// Yedekten geri yükleme işlemi yapar (gösterim amaçlı).
        /// </summary>
        Task<bool> RestoreDatabaseAsync(int userId, string backupFilePath);

      
      
    }
}
