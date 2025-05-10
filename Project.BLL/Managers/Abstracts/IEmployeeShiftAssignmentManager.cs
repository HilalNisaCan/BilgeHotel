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
            /// Belirli çalışana belirtilen vardiyayı atar. Aynı güne tekrar atama yapılmaz.
            /// </summary>
            Task<bool> AssignShiftAsync(int employeeId, int shiftId, DateTime date);

            /// <summary>
            /// Belirli çalışanın haftalık vardiya atamalarını getirir.
            /// </summary>
            /// <summary>
            /// Çalışanın bir haftalık vardiya atamalarını getirir.
            /// </summary>
            Task<List<EmployeeShiftAssignmentDto>> GetAssignmentsForWeekAsync(int employeeId, DateTime weekStartDate);
        


    }
}
