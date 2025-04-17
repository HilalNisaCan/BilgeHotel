using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    /// <summary>
    /// Oda bakım/arıza işlemlerini yöneten iş akışı arayüzü.
    /// </summary>
    public interface IRoomMaintenanceManager : IManager<RoomMaintenanceDto, RoomMaintenance>
    {
        /// <summary>
        /// Oda için belirli tarihte bakım planlar.
        /// </summary>
        Task<int> ScheduleRoomMaintenanceAsync(int roomId, DateTime maintenanceDate, MaintenanceType type);

        /// <summary>
        /// Bakım kaydının durumunu günceller.
        /// </summary>
        Task<bool> UpdateMaintenanceStatusAsync(int maintenanceId, MaintenanceStatus status);

        /// <summary>
        /// Belirli bir oda için tüm bakım geçmişini getirir.
        /// </summary>
        Task<List<RoomMaintenanceDto>> GetMaintenanceHistoryByRoomAsync(int roomId);

        /// <summary>
        /// Bakım kaydını sistemden siler.
        /// </summary>
        Task<bool> DeleteMaintenanceRecordAsync(int maintenanceId);

        /// <summary>
        /// Devam eden (Pending/InProgress) bakım kayıtlarını getirir.
        /// </summary>
        Task<List<RoomMaintenanceDto>> GetActiveMaintenancesAsync();

        /// <summary>
        /// Tek bir bakım kaydını getirir.
        /// </summary>
        Task<RoomMaintenanceDto> GetMaintenanceByIdAsync(int id);
    }
}
