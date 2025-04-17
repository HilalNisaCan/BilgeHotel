using AutoMapper;
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

namespace Project.BLL.Managers.Concretes
{
    /// <summary>
    /// Oda bakım ve arıza işlemlerini yöneten manager sınıfı.
    /// </summary>
    public class RoomMaintenanceManager : BaseManager<RoomMaintenanceDto, RoomMaintenance>, IRoomMaintenanceManager
    {
        private readonly IRoomMaintenanceRepository _maintenanceRepository;
        private readonly IMapper _mapper;

        public RoomMaintenanceManager(IRoomMaintenanceRepository maintenanceRepository, IMapper mapper)
            : base(maintenanceRepository, mapper)
        {
            _maintenanceRepository = maintenanceRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Oda için belirli tarihte bakım planlar.
        /// </summary>
        public async Task<int> ScheduleRoomMaintenanceAsync(int roomId, DateTime maintenanceDate, MaintenanceType type)
        {
            var entity = new RoomMaintenance
            {
                RoomId = roomId,
                ScheduledDate = maintenanceDate,
                MaintenanceType = type,
                MaintenanceStatus = MaintenanceStatus.Pending
            };

            await _maintenanceRepository.AddAsync(entity);
            return entity.Id;
        }

        /// <summary>
        /// Bakım kaydının durumunu günceller.
        /// </summary>
        public async Task<bool> UpdateMaintenanceStatusAsync(int maintenanceId, MaintenanceStatus status)
        {
            var entity = await _maintenanceRepository.GetByIdAsync(maintenanceId);
            if (entity == null) return false;

            entity.MaintenanceStatus = status;
            await _maintenanceRepository.UpdateAsync(entity);
            return true;
        }

        /// <summary>
        /// Belirli odaya ait bakım geçmişini getirir.
        /// </summary>
        public async Task<List<RoomMaintenanceDto>> GetMaintenanceHistoryByRoomAsync(int roomId)
        {
            var list = await _maintenanceRepository.GetAllAsync(x => x.RoomId == roomId);
            return _mapper.Map<List<RoomMaintenanceDto>>(list);
        }

        /// <summary>
        /// Bakım kaydını siler.
        /// </summary>
        public async Task<bool> DeleteMaintenanceRecordAsync(int maintenanceId)
        {
            var entity = await _maintenanceRepository.GetByIdAsync(maintenanceId);
            if (entity == null) return false;

            await _maintenanceRepository.RemoveAsync(entity);
            return true;
        }

        /// <summary>
        /// Tamamlanmamış tüm bakım kayıtlarını getirir.
        /// </summary>
        public async Task<List<RoomMaintenanceDto>> GetActiveMaintenancesAsync()
        {
            var list = await _maintenanceRepository.GetAllAsync(x => x.MaintenanceStatus != MaintenanceStatus.Completed);
            return _mapper.Map<List<RoomMaintenanceDto>>(list);
        }

        /// <summary>
        /// Tekil bakım kaydını getirir.
        /// </summary>
        public async Task<RoomMaintenanceDto> GetMaintenanceByIdAsync(int id)
        {
            var entity = await _maintenanceRepository.GetByIdAsync(id);
            return _mapper.Map<RoomMaintenanceDto>(entity);
        }

        /// <summary>
        /// Eğer bugün için aynı oda ve bakım tipiyle kayıt varsa getirir. Yoksa yeni kayıt oluşturur.
        /// </summary>
        public async Task<int> GetOrCreateTodayMaintenanceAsync(int roomId, MaintenanceType type)
        {
            DateTime today = DateTime.Today;

            // Bugün için aynı oda ve bakım tipinde varsa
            RoomMaintenance? existing = await _maintenanceRepository.GetFirstOrDefaultAsync(
                x => x.RoomId == roomId &&
                     x.MaintenanceType == type &&
                     x.ScheduledDate.Date == today &&
                     x.Status != DataStatus.Deleted,
                include: null // Include yok, sadece kontrol ediyoruz
            );

            if (existing != null)
                return existing.Id;

            // Yeni kayıt oluşturuluyor
            RoomMaintenance entity = new RoomMaintenance
            {
                RoomId = roomId,
                MaintenanceType = type,
                ScheduledDate = today,
                StartDate = DateTime.Now,
                MaintenanceStatus = MaintenanceStatus.Scheduled,
                Status = DataStatus.Inserted
            };

            await _maintenanceRepository.AddAsync(entity);
            return entity.Id;
        }
    }
}
