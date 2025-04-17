using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IRoomCleaningScheduleRepository:IRepository<RoomCleaningSchedule>
    {
        Task<List<RoomCleaningSchedule>> GetPendingCleaningsAsync(); // Temizliği bekleyen odaları getir
        Task<List<RoomCleaningSchedule>> GetCompletedCleaningsAsync(); // Temizliği tamamlanmış odaları getir
        Task<List<RoomCleaningSchedule>> GetCleaningsByDateAsync(DateTime date); // Belirli bir tarihte yapılan temizlemeleri getir
        Task<List<RoomCleaningSchedule>> GetCleaningsByEmployeeIdAsync(int employeeId); // Belirli bir çalışanın yaptığı temizlikleri getir
    }
}
