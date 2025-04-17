using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
{ /// <summary>
  /// Oda temizlik planlama ve temizlik sonrası durumu yöneten manager sınıfıdır.
  /// </summary>
    public class RoomCleaningScheduleManager : BaseManager<RoomCleaningScheduleDto, RoomCleaningSchedule>, IRoomCleaningScheduleManager
    {
        private readonly IRoomCleaningScheduleRepository _cleaningScheduleRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomCleaningScheduleManager(IRoomCleaningScheduleRepository cleaningScheduleRepository, IRoomRepository roomRepository, IMapper mapper)
            : base(cleaningScheduleRepository, mapper)
        {
            _cleaningScheduleRepository = cleaningScheduleRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Belirli bir oda için temizlik planı oluşturur.
        /// </summary>
        public async Task<int> ScheduleRoomCleaningAsync(int roomId, DateTime cleaningDate)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                throw new Exception("Oda bulunamadı!");

            var cleaningSchedule = new RoomCleaningSchedule
            {
                RoomId = roomId,
                ScheduledDate = cleaningDate,
                CleaningStatus = CleaningStatus.Scheduled // Planlandı
            };

            await _cleaningScheduleRepository.AddAsync(cleaningSchedule);
            return cleaningSchedule.Id;
        }

        /// <summary>
        /// Temizlik tamamlandı olarak işaretlenir ve oda durumu "Boş" yapılır.
        /// </summary>
        public async Task<bool> MarkCleaningAsCompletedAsync(int cleaningScheduleId)
        {
            var cleaningSchedule = await _cleaningScheduleRepository.GetByIdAsync(cleaningScheduleId);
            if (cleaningSchedule == null)
                return false;

            cleaningSchedule.CleaningStatus = CleaningStatus.Completed;
            await _cleaningScheduleRepository.UpdateAsync(cleaningSchedule);

            // Oda durumunu "Available" olarak güncelle
            var room = await _roomRepository.GetByIdAsync(cleaningSchedule.RoomId);
            room.RoomStatus = RoomStatus.Available;
            await _roomRepository.UpdateAsync(room);

            return true;
        }

        /// <summary>
        /// Belirli bir tarihte planlanan temizlik işlemlerini listeler.
        /// </summary>
        public async Task<List<RoomCleaningScheduleDto>> GetScheduledCleaningsAsync(DateTime date)
        {
            var cleanings = await _cleaningScheduleRepository.GetAllAsync(c => c.ScheduledDate.Date == date.Date);
            return _mapper.Map<List<RoomCleaningScheduleDto>>(cleanings);
        }

        public async Task<RoomCleaningScheduleDto?> GetLatestByRoomIdAsync(int roomId)
        {
            // RoomId'ye ait aktif temizlik kayıtlarını ve çalışanı dahil ederek getiriyoruz
            List<RoomCleaningSchedule> schedules = (await _cleaningScheduleRepository
                .GetAllWithIncludeAsync(
                    predicate: x => x.RoomId == roomId && x.Status != DataStatus.Deleted, // aktif kayıtlar
                    include: q => q.Include(x => x.AssignedEmployee)
                )).ToList();

            // Kayıt varsa: oluşturulma tarihine göre en son olanı al
            RoomCleaningSchedule? latest = schedules
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefault();

            return latest == null
                ? null
                : _mapper.Map<RoomCleaningScheduleDto>(latest);
        }

        public async Task<bool> CreateAndConfirmAsync(RoomCleaningSchedule entity)
        {
            return await _cleaningScheduleRepository.CreateAndConfirmAsync(entity);
        }
    }
}
