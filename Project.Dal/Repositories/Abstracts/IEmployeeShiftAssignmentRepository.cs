using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IEmployeeShiftAssignmentRepository:IRepository<EmployeeShiftAssignment>
    {
        Task<List<EmployeeShiftAssignment>> GetAssignmentsByEmployeeIdAsync(int employeeId); // Çalışanın vardiya atamalarını getir
        Task<List<EmployeeShiftAssignment>> GetAssignmentsByShiftIdAsync(int shiftId); // Belirli bir vardiyaya atanmış çalışanları getir
        Task<EmployeeShiftAssignment> GetAsync(Expression<Func<EmployeeShiftAssignment, bool>> predicate); // ✅ Vardiya atanmasını kontrol etmek için
        Task<List<EmployeeShiftAssignment>> GetAllWithIncludeAsync();

        Task<List<EmployeeShiftAssignment>> GetAllWithEmployeeAndShiftAsync();
    }
}

