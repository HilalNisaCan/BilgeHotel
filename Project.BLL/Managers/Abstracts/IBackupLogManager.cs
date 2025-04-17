using Project.BLL.DtoClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    //public interface IBackupLogManager : IManager<BackupLogDto, BackupLog>
    //{
    //    /// <summary>
    //    /// Kullanıcı tarafından manuel veritabanı yedeği alır.
    //    /// </summary>
    //    Task<bool> BackupDatabaseAsync(int userId, string backupFolderPath);

    //    /// <summary>
    //    /// Manuel yedekten veritabanını geri yükler.
    //    /// </summary>
    //    Task<bool> RestoreDatabaseAsync(int userId, string backupFilePath);

    //    /// <summary>
    //    /// Tüm yedekleme geçmişini DTO olarak döner.
    //    /// </summary>
    //    Task<List<BackupLogDto>> GetBackupHistoryAsync();

    //    /// <summary>
    //    /// Sistem tarafından planlanmış otomatik yedeklemeyi başlatır.
    //    /// </summary>
    //    Task ScheduleAutomaticBackupAsync();

    //    Task<bool> CreateBackAsync(BackupLogDto dto);
    //}
}
