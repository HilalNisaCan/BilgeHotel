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
    public class RoomMaintenanceAssignmentRepository : BaseRepository<RoomMaintenanceAssignment>, IRoomMaintenanceAssignmentRepository
    {
        public RoomMaintenanceAssignmentRepository(MyContext context) : base(context) { }

        public async Task<List<RoomMaintenanceAssignment>> GetAssignmentsByEmployeeIdAsync(int employeeId)
        {
            return await _dbSet.Where(rma => rma.EmployeeId == employeeId).ToListAsync();
        }

        public async Task<List<RoomMaintenanceAssignment>> GetAssignmentsByMaintenanceIdAsync(int maintenanceId)
        {
            return await _dbSet.Where(rma => rma.RoomMaintenanceId == maintenanceId).ToListAsync();
        }
    }
}
