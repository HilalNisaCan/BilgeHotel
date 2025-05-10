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
    /// Vardiya yönetimi iş akışlarını tanımlar.
    /// </summary>
    public interface IEmployeeShiftManager : IManager<EmployeeShiftDto, EmployeeShift>
    {
        /// <summary>
        /// Çalışana yeni vardiya atar.
        /// </summary>
        Task<bool> AssignShiftAsync(int employeeId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Yeni vardiya oluşturur ve ID’yi döner.
        /// </summary>
        Task<int> CreateAndReturnIdAsync(EmployeeShiftDto dto);

        /// <summary>
        /// Tüm vardiyalarla birlikte atanan çalışanları getirir.
        /// </summary>
        Task<List<EmployeeShiftDto>> GetAllWithAssignmentsAsync();

        /// <summary>
        /// Belirtilen aralıkta çalışanın fazla mesaisini hesaplar.
        /// </summary>
        Task<double> CalculateOvertimeAsync(int employeeId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Belirtilen aralıkta çalışanın maaş bilgisini hesaplar.
        /// </summary>
        Task<(decimal Salary, double TotalWorkedHours)> CalculateSalaryAsync(int employeeId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Vardiyayı siler.
        /// </summary>
        Task<bool> DeleteShiftByIdAsync(int shiftId);

        /// <summary>
        /// Vardiya bilgilerini günceller.
        /// </summary>
        Task<bool> UpdateShiftAsync(EmployeeShiftDto dto);
    }
}
