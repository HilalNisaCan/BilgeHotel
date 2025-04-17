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
    /// Çalışanlara vardiya atama iş akışlarını tanımlar.
    /// </summary>
    public interface IEmployeeShiftAssignmentManager : IManager<EmployeeShiftAssignmentDto, EmployeeShiftAssignment>
    {
        /// <summary>
        /// Belirli bir çalışana belirtilen vardiyayı atar. Aynı güne tekrar atama yapılmaz.
        /// </summary>
        Task<bool> AssignShiftAsync(int employeeId, int shiftId, DateTime date);

        /// <summary>
        /// Vardiya atamasını sistemden kaldırır.
        /// </summary>
        Task<bool> RemoveShiftAssignmentAsync(int assignmentId);

        /// <summary>
        /// Belirli çalışanın tüm vardiya atamalarını getirir.
        /// </summary>
        Task<List<EmployeeShiftAssignmentDto>> GetEmployeeShiftsAsync(int employeeId);

        /// <summary>
        /// Aynı vardiya tekrar atanacak mı kontrol eder (çakışma önleme).
        /// </summary>
        Task<bool> ValidateShiftAssignmentAsync(int employeeId, DateTime date);

        Task<List<EmployeeShiftAssignmentDto>> GetAllWithDetailsAsync();

        Task<List<EmployeeShiftAssignmentDto>> GetAssignmentsForWeekAsync(int employeeId, DateTime weekStartDate);

        
    }
}
