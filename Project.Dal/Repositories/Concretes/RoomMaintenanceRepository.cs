using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class RoomMaintenanceRepository : BaseRepository<RoomMaintenance>, IRoomMaintenanceRepository
    {
        public RoomMaintenanceRepository(MyContext context) : base(context) { }

        // Belirli bir odanın bakım kayıtlarını getir
        public async Task<List<RoomMaintenance>> GetMaintenancesByRoomIdAsync(int roomId)
        {
            return await _dbSet.Where(rm => rm.RoomId == roomId).ToListAsync();
        }

        // Bekleyen (Tamamlanmamış) bakım işlemlerini getir
        public async Task<List<RoomMaintenance>> GetPendingMaintenancesAsync()
        {
            return await _dbSet.Where(rm => rm.MaintenanceStatus != MaintenanceStatus.Completed).ToListAsync();
        }

        // Belirtilen tarihte başlayan bakım işlemlerini getir
        public async Task<List<RoomMaintenance>> GetMaintenancesByDateAsync(DateTime date)
        {
            return await _dbSet.Where(rm => rm.StartDate.Date == date.Date).ToListAsync();
        }

        // Bakımı tamamlanmış olarak işaretle
        public async Task<bool> MarkMaintenanceAsCompletedAsync(int maintenanceId)
        {
            var maintenance = await _dbSet.FindAsync(maintenanceId);
            if (maintenance == null) return false;

            maintenance.MaintenanceStatus = MaintenanceStatus.Completed;
            maintenance.EndDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
