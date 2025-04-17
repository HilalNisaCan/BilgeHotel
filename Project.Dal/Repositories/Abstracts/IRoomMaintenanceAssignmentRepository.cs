using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IRoomMaintenanceAssignmentRepository:IRepository<RoomMaintenanceAssignment>
    {
        Task<List<RoomMaintenanceAssignment>> GetAssignmentsByEmployeeIdAsync(int employeeId); // Belirli bir çalışana atanmış bakım işlemlerini getir
        Task<List<RoomMaintenanceAssignment>> GetAssignmentsByMaintenanceIdAsync(int maintenanceId); // Belirli bir bakım işlemine atanmış çalışanları getir
       
    }
}
