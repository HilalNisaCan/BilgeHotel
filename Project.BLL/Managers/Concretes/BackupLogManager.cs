using AutoMapper;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    /// <summary>
    /// Veritabanı yedekleme ve geri yükleme işlemlerini yöneten manager sınıfı.
    /// (Simülasyon amaçlı, gerçek yedekleme yapılmaz)
    /// </summary>
    public class BackupLogManager : BaseManager<BackupLogDto, BackupLog>, IBackupLogManager
    {
        private readonly IBackUpRepository _backupLogRepository;
        private readonly IMapper _mapper;

        public BackupLogManager(IBackUpRepository backupLogRepository, IMapper mapper)
            : base(backupLogRepository, mapper)
        {
            _backupLogRepository = backupLogRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yedekleme işlemi başlatır (simülasyon).
        /// </summary>
        public async Task<bool> BackupDatabaseAsync(int userId, string backupFolderPath)
        {
            string filePath = Path.Combine(backupFolderPath, $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

            BackupLog log = new BackupLog
            {
                UserId = userId,
                IsAuthorized = true,
                FilePath = filePath,
                BackupDate = DateTime.Now,
                BackupStatus = BackupStatus.Success,
                IsRestored = false,
                IPAddress = GetIpAddress(),
                MachineName = Environment.MachineName
            };

            await _backupLogRepository.AddAsync(log);
            return true;
        }

        /// <summary>
        /// Geri yükleme işlemi başlatır (simülasyon).
        /// </summary>
        public async Task<bool> RestoreDatabaseAsync(int userId, string backupFilePath)
        {
            BackupLog log = new BackupLog
            {
                UserId = userId,
                IsAuthorized = true,
                FilePath = backupFilePath,
                BackupDate = DateTime.Now.AddDays(-1), // Simülasyon olarak dün alınmış gibi
                BackupStatus = BackupStatus.Success,
                IsRestored = true,
                RestoredDate = DateTime.Now,
                IPAddress = GetIpAddress(),
                MachineName = Environment.MachineName
            };

            await _backupLogRepository.AddAsync(log);
            return true;
        }

        /// <summary>
        /// (Mock) IP adresi alır — gerçek projede HttpContext üzerinden alınmalı.
        /// </summary>
        private string GetIpAddress()
        {
            return "127.0.0.1"; // placeholder IP
        }
    }
}
