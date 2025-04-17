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
    /// Oda bakım işlemlerinin personele atanmasıyla ilgili iş akışlarını tanımlar.
    /// </summary>
    public interface IRoomMaintenanceAssignmentManager : IManager<RoomMaintenanceAssignmentDto, RoomMaintenanceAssignment>
    {
        /// <summary>
        /// Belirli bir bakım işlemini ilgili çalışana atar.
        /// </summary>
        Task<bool> AssignMaintenanceAsync(int maintenanceId, int employeeId);

        /// <summary>
        /// Atanmış bir bakım işlemini iptal eder.
        /// </summary>
        Task<bool> RemoveMaintenanceAssignmentAsync(int assignmentId);

        /// <summary>
        /// Belirli bir odaya ait tüm bakım atamalarını getirir.
        /// </summary>
        Task<List<RoomMaintenanceAssignmentDto>> GetRoomMaintenanceAssignmentsAsync(int roomId);

        /// <summary>
        /// Bakım atamasını tamamlanmış olarak işaretler.
        /// </summary>
        Task<bool> CompleteMaintenanceAsync(int assignmentId);

        Task<bool> AssignOrCreateMaintenanceAsync(int roomId, int employeeId, DateTime date, string? description);

        /// <summary>
        /// Verilen odaya ait en son bakım atamasını döndürür (son atama tarihi baz alınır).
        /// </summary>
        Task<RoomMaintenanceAssignmentDto?> GetLatestByRoomIdAsync(int roomId);

        Task<bool> CreateWithEntityAsync(RoomMaintenanceAssignment entity);
    }
}
