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
        /// Eğer bugün için aynı oda ve bakım tipiyle bir kayıt varsa onu döner.
        /// Eğer yoksa yeni bir bakım kaydı oluşturur.
        /// </summary>
        /// <param name="roomId">İşlem yapılacak odanın ID'si</param>
        /// <param name="type">Bakım tipi (örnek: Elektrik, Tesisat, Temizlik)</param>
        /// <returns>Mevcut ya da yeni oluşturulan bakım kaydının ID'si</returns>
        /// 
        /// <remarks>
        /// 📌 Not: Bu metot, günlük tekrar eden bakım kayıtlarını engellemek için kullanılır.
        /// Aynı gün içinde aynı odada aynı türde birden fazla bakım oluşturulmasını önler.
        /// Arka planda otomasyon, API veya planlayıcı servisleriyle birlikte kullanılabilir.
        ///  “günlük bakım tekilliği garantisi” örneği olarak anlatılabilir.
        /// </remarks>
        public async Task<int> GetOrCreateTodayMaintenanceAsync(int roomId, MaintenanceType type)
        {
            DateTime today = DateTime.Today;

            // 📌 Aynı gün içinde aynı oda ve bakım türünde bir kayıt var mı kontrol edilir
            RoomMaintenance? existing = await _maintenanceRepository.GetFirstOrDefaultAsync(
                x => x.RoomId == roomId &&
                     x.MaintenanceType == type &&
                     x.ScheduledDate.Date == today &&
                     x.Status != DataStatus.Deleted,
                include: null // include gerekli değil
            );

            // ✅ Varsa mevcut ID döndürülür
            if (existing != null)
                return existing.Id;

            // ❗ Yoksa yeni kayıt oluşturulur
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
