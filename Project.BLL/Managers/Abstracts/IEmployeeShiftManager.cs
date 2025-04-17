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
        /// Çalışana yeni vardiya ataması yapar.
        /// </summary>
        Task<bool> AssignShiftAsync(int employeeId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Çalışanın haftalık toplam çalışma saatini hesaplar.
        /// </summary>
        Task<double> CalculateWeeklyHoursAsync(int employeeId, DateTime weekStartDate);

        /// <summary>
        /// Fazla mesai süresini hesaplar. (40 saati aşan)
        /// </summary>
      //  Task<double> CalculateOvertimeAsync(int employeeId, DateTime weekStartDate);

        /// <summary>
        /// Aylık maaşı hesaplar (fazla mesai dahil).
        /// </summary>
     //   Task<decimal> CalculateSalaryAsync(int employeeId, int month, int year);

        /// <summary>
        /// Belirli bir tarihi çalışan için izin günü olarak işaretler.
        /// </summary>
        Task<bool> SetDayOffAsync(int employeeId, DateTime day);

        Task<int> CreateAndReturnIdAsync(EmployeeShiftDto dto);

        /// <summary>
        /// Tüm vardiyalarla birlikte bu vardiyaya atanmış çalışanları getirir.
        /// </summary>
        Task<List<EmployeeShiftDto>> GetAllWithAssignmentsAsync();

        Task<double> CalculateOvertimeAsync(int employeeId, DateTime startDate, DateTime endDate);

        Task<(decimal Salary, double TotalWorkedHours)> CalculateSalaryAsync(int employeeId, DateTime startDate, DateTime endDate);
        /// <summary>
        /// Belirtilen ID’ye sahip vardiyayı sistemden siler.
        /// </summary>
        /// <param name="shiftId">Silinecek vardiyanın ID’si</param>
        /// <returns>İşlem başarılıysa true, aksi halde false döner.</returns>
        Task<bool> DeleteShiftByIdAsync(int shiftId);

        Task<bool> UpdateShiftAsync(EmployeeShiftDto dto);
    }
}
