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
    /// Oda temizlik planlama ve takibini yöneten iş kurallarını tanımlar.
    /// </summary>
    public interface IRoomCleaningScheduleManager : IManager<RoomCleaningScheduleDto, RoomCleaningSchedule>
    {


        /// <summary>
        /// Belirtilen odaya ait en son temizlik planını getirir.
        /// </summary>
        /// <param name="roomId">Oda ID’si</param>
        /// <returns>Son temizlik planı DTO’su, yoksa null</returns>
        /// 
        /// <remarks>
        /// 📌 Not: Bu metot, oda detay ekranında en son temizlik bilgisi göstermek için kullanılır.
        /// Temizlik görevlisi atama veya temizlik geçiş ekranlarında da değerlendirilebilir.
        /// </remarks>
        Task<RoomCleaningScheduleDto?> GetLatestByRoomIdAsync(int roomId);

        /// <summary>
        /// Yeni bir temizlik planı oluşturur ve ardından temizlenmiş olarak işaretler.
        /// </summary>
        /// <param name="dto">Oda temizlik DTO’su</param>
        /// <returns>İşlem başarılıysa true, değilse false</returns>
        /// 
        /// <remarks>
        /// 📌 Not: Bu işlem tek adıma indirgenmiştir — planlama ve onay tek hamlede yapılır.
        /// Kat görevlisi işlemlerinde hızlı temizlik tamamlaması senaryosu için uygundur.
        /// </remarks>
        Task<bool> CreateAndConfirmAsync(RoomCleaningScheduleDto dto);

    }
}
