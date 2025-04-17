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
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(MyContext context) : base(context)
        {
        }

        public async Task<Employee> GetEmployeeWithShiftsAsync(int employeeId)
        {
            return await _dbSet.Include(e => e.EmployeeShiftAssignments)
                               .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task<List<Employee>> GetEmployeesByPositionAsync(EmployeePosition position)
        {
            return await _dbSet.Where(e => e.Position == position).ToListAsync();
        }
        public async Task<List<Employee>> GetAllEmployeesWithUserAsync()
        {
            return await _dbSet
                .Include(e => e.User)
                    .ThenInclude(u => u.UserProfile)
                .ToListAsync();
        }
    }
}
