using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class EmployeeShiftAssignmentRepository : BaseRepository<EmployeeShiftAssignment>, IEmployeeShiftAssignmentRepository
    {
        public EmployeeShiftAssignmentRepository(MyContext context) : base(context)
        {
        }

        public async Task<List<EmployeeShiftAssignment>> GetAllWithEmployeeAndShiftAsync()
        {
            return await _context.EmployeeShiftAssignments
              .Include(x => x.Employee)
              .Include(x => x.EmployeeShift)
               .Where(x => x.Status == DataStatus.Inserted)
               .AsNoTracking() // ✅ Proxy olmadan düz nesne döner
                .ToListAsync();
        }

        public async Task<List<EmployeeShiftAssignment>> GetAllWithIncludeAsync()
        {
            return await _context.EmployeeShiftAssignments
                .Include(e => e.Employee)
                .Include(e => e.EmployeeShift)
                .ToListAsync();
        }

        public async Task<List<EmployeeShiftAssignment>> GetAssignmentsByEmployeeIdAsync(int employeeId)
        {
            return await _dbSet.Where(esa => esa.EmployeeId == employeeId).ToListAsync();
        }

        public async Task<List<EmployeeShiftAssignment>> GetAssignmentsByShiftIdAsync(int shiftId)
        {
            return await _dbSet.Where(esa => esa.EmployeeShiftId == shiftId).ToListAsync();
        }

        public async Task<EmployeeShiftAssignment> GetAsync(Expression<Func<EmployeeShiftAssignment, bool>> predicate)
        {
            return await _context.Set<EmployeeShiftAssignment>().FirstOrDefaultAsync(predicate);
        }
    }
}
