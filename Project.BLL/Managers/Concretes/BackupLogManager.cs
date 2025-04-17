using AutoMapper;
using Microsoft.Data.SqlClient;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Project.BLL.Managers.Concretes
//{
////    public class BackupLogManager : BaseManager<BackupLogDto, BackupLog>, IBackupLogManager
//    {
//        private readonly IBackUpRepository _backupRepository;
//        private readonly IUserRepository _userRepository;
//        private readonly string _connectionString;
//        private readonly IMapper _mapper;

//        public BackupLogManager(IBackUpRepository backupRepository,
//                                IUserRepository userRepository,
//                                string connectionString,
//                                IMapper mapper)
//            : base(backupRepository, mapper)
//        {
//            _backupRepository = backupRepository;
//            _userRepository = userRepository;
//            _connectionString = connectionString;
//            _mapper = mapper;
//        }

//        /// <summary>
//        /// Belirtilen kullanıcı tarafından veritabanı yedeği alır. Sadece Admin yetkisine izin verilir.
//        /// </summary>
//        public async Task<bool> BackupDatabaseAsync(int userId, string backupFolderPath)
//        {
//            var user = await _userRepository.GetByIdAsync(userId);
//            if (user == null || user.Role != UserRole.Admin) // ✅ Yalnızca Admin izinli
//                throw new UnauthorizedAccessException("Bu işlemi yapma yetkiniz yok!");

//            string backupFilePath = Path.Combine(backupFolderPath, $"Backup_{DateTime.UtcNow:yyyyMMddHHmmss}.bak");
//            string query = $"BACKUP DATABASE BilgeHotel TO DISK = '{backupFilePath}'";

//            try
//            {
//                using (SqlConnection conn = new SqlConnection(_connectionString))
//                {
//                    await conn.OpenAsync();
//                    using (SqlCommand cmd = new SqlCommand(query, conn))
//                    {
//                        await cmd.ExecuteNonQueryAsync();
//                    }
//                }

//                var backupLog = new BackupLog
//                {
//                    FilePath = backupFilePath,
//                    CreatedDate = DateTime.UtcNow,
//                    BackupStatus = BackupStatus.Success
//                };

//                await _backupRepository.AddAsync(backupLog);
//                return true;
//            }
//            catch
//            {
//                var backupLog = new BackupLog
//                {
//                    FilePath = backupFilePath,
//                    CreatedDate = DateTime.UtcNow,
//                    BackupStatus = BackupStatus.Failed

//                };

//                await _backupRepository.AddAsync(backupLog);
//                return false;
//            }
//        }

//        public Task<bool> CreateBackAsync(BackupLogDto dto)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// Tüm yedekleme geçmişini DTO olarak getirir.
//        /// </summary>
//        public async Task<List<BackupLogDto>> GetBackupHistoryAsync()
//        {
//            var history = await _backupRepository.GetAllAsync();
//            return _mapper.Map<List<BackupLogDto>>(history);
//        }

//        /// <summary>
//        /// Verilen yedek dosyasını kullanarak veritabanını geri yükler. Sadece Admin yetkisine izin verilir.
//        /// </summary>
//        public async Task<bool> RestoreDatabaseAsync(int userId, string backupFilePath)
//        {
//            var user = await _userRepository.GetByIdAsync(userId);
//            if (user == null || user.Role != UserRole.Admin) // ✅ Yalnızca Admin izinli
//                throw new UnauthorizedAccessException("Bu işlemi yapma yetkiniz yok!");

//            string query = $"RESTORE DATABASE BilgeHotel FROM DISK = '{backupFilePath}' WITH REPLACE";

//            try
//            {
//                using (SqlConnection conn = new SqlConnection(_connectionString))
//                {
//                    await conn.OpenAsync();
//                    using (SqlCommand cmd = new SqlCommand(query, conn))
//                    {
//                        await cmd.ExecuteNonQueryAsync();
//                    }
//                }
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        /// <summary>
//        /// Sistem tarafından otomatik olarak yedekleme planlar (adminId ve yol daha sonra konfigürasyona taşınmalı).
//        /// </summary>
//        public async Task ScheduleAutomaticBackupAsync()
//        {
//            string backupFolderPath = "C:\\DatabaseBackups";
//            int adminUserId = 1; // FIXME: Yapılandırmadan alınmalı
//            await BackupDatabaseAsync(adminUserId, backupFolderPath);
//        }

//    }
//}
