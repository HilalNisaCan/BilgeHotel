using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class RoomCleaningScheduleRepository : BaseRepository<RoomCleaningSchedule>, IRoomCleaningScheduleRepository
    {
        public RoomCleaningScheduleRepository(MyContext context) : base(context)
        {
        }

        // Temizliği bekleyen odaları getir
        public async Task<List<RoomCleaningSchedule>> GetPendingCleaningsAsync()
        {
            return await _dbSet
                .Where(rcs => !rcs.IsCompleted)
                .ToListAsync();
        }

        // Temizliği tamamlanmış odaları getir
        public async Task<List<RoomCleaningSchedule>> GetCompletedCleaningsAsync()
        {
            return await _dbSet
                .Where(rcs => rcs.IsCompleted)
                .ToListAsync();
        }

        // Belirli bir tarihte yapılan temizlemeleri getir
        public async Task<List<RoomCleaningSchedule>> GetCleaningsByDateAsync(DateTime date)
        {
            return await _dbSet
                .Where(rcs => rcs.ScheduledDate.Date == date.Date)
                .ToListAsync();
        }

        // Belirli bir çalışanın yaptığı temizlikleri getir
        public async Task<List<RoomCleaningSchedule>> GetCleaningsByEmployeeIdAsync(int employeeId)
        {
            return await _dbSet
                .Where(rcs => rcs.AssignedEmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<bool> CreateAndConfirmAsync(RoomCleaningSchedule entity)
        {
            entity.CreatedDate = DateTime.Now;
            await _context.RoomCleaningSchedules.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
