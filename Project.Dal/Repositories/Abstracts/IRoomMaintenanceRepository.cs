using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IRoomMaintenanceRepository:IRepository<RoomMaintenance>
    {
        Task<List<RoomMaintenance>> GetMaintenancesByRoomIdAsync(int roomId); // Belirli bir odaya ait tüm bakım kayıtlarını getir
        Task<List<RoomMaintenance>> GetPendingMaintenancesAsync(); // Tamamlanmamış bakım kayıtlarını getir
        Task<List<RoomMaintenance>> GetMaintenancesByDateAsync(DateTime date); // Belirli bir tarihte planlanan bakım işlemlerini getir
        Task<bool> MarkMaintenanceAsCompletedAsync(int maintenanceId); // Bakımı tamamlandı olarak işaretle
    }
}
