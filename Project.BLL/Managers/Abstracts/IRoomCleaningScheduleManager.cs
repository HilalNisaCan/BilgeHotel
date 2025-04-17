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
        /// Belirli bir oda için temizlik planı oluşturur.
        /// </summary>
        Task<int> ScheduleRoomCleaningAsync(int roomId, DateTime cleaningDate);

        /// <summary>
        /// Planlanan temizlik işlemini tamamlanmış olarak işaretler.
        /// </summary>
        Task<bool> MarkCleaningAsCompletedAsync(int cleaningScheduleId);

        /// <summary>
        /// Belirli bir tarihte planlanan tüm temizlikleri listeler.
        /// </summary>
        Task<List<RoomCleaningScheduleDto>> GetScheduledCleaningsAsync(DateTime date);
    }
}
