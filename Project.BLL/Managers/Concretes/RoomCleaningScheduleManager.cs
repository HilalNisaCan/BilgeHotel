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
     
        private readonly IMapper _mapper;

        public RoomCleaningScheduleManager(IRoomCleaningScheduleRepository cleaningScheduleRepository,  IMapper mapper)
            : base(cleaningScheduleRepository, mapper)
        {
            _cleaningScheduleRepository = cleaningScheduleRepository;
            _mapper = mapper;
        }



        /// <summary>
        /// Belirtilen odaya ait en son temizlik planını getirir.
        /// </summary>
        /// <param name="roomId">Temizlik geçmişi istenen odanın ID'si</param>
        /// <returns>Son temizlik planı varsa DTO olarak döner, yoksa null</returns>
        /// 
        /// <remarks>
        /// 📌 Not: Bu metot, bir odada yapılan en güncel temizlik işlemini görmek için kullanılır.  
        /// Kullanım senaryoları: Oda detay ekranı, temizlik planlama modülü, görevli geçmiş kontrolü.
        /// </remarks>
        public async Task<RoomCleaningScheduleDto?> GetLatestByRoomIdAsync(int roomId)
        {
            // RoomId'ye ait silinmemiş temizlik kayıtları ve görevli personel bilgisiyle alınır
            List<RoomCleaningSchedule> schedules = (await _cleaningScheduleRepository
                .GetAllWithIncludeAsync(
                    predicate: x => x.RoomId == roomId && x.Status != DataStatus.Deleted,
                    include: q => q.Include(x => x.AssignedEmployee)
                )).ToList();

            // Oluşturulma tarihine göre en güncel kayıt seçilir
            RoomCleaningSchedule? latest = schedules
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefault();

            return latest == null
                ? null
                : _mapper.Map<RoomCleaningScheduleDto>(latest);
        }


        /// <summary>
        /// Yeni bir temizlik planı oluşturur ve aynı anda "tamamlandı" olarak işaretler.
        /// </summary>
        /// <param name="dto">Temizlik işlemi DTO verisi</param>
        /// <returns>Başarılıysa true, aksi halde false</returns>
        /// 
        /// <remarks>
        /// 📌 Not: Bu işlem “tek tıkla temizlik” gibi bir yapıya uygundur.  
        /// Kat görevlisi sisteme girdiğinde hem temizlik planı yapılır hem de sistem onu otomatik temizlenmiş olarak işaretler.
        /// </remarks>
        public async Task<bool> CreateAndConfirmAsync(RoomCleaningScheduleDto dto)
        {
            RoomCleaningSchedule entity = _mapper.Map<RoomCleaningSchedule>(dto);
            return await _cleaningScheduleRepository.CreateAndConfirmAsync(entity);
        }
    }
}
