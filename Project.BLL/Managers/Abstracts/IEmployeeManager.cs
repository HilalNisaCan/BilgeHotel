using Project.BLL.DtoClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    public interface IEmployeeManager : IManager<EmployeeDto, Employee>
    {
        /// <summary>
        /// Yeni bir çalışan oluşturur.
        /// </summary>
        /// <param name="dto">Çalışan DTO bilgileri</param>
        /// <returns>Oluşturulan çalışanın ID'si</returns>
        Task<int> AddEmployeeAsync(EmployeeDto dto);

        /// <summary>
        /// Çalışan bilgilerini günceller.
        /// </summary>
        /// <param name="dto">Güncellenmiş DTO</param>
        /// <returns>Başarı durumu</returns>
        Task<bool> UpdateEmployeeAsync(EmployeeDto dto);

        /// <summary>
        /// ID ile çalışan getirir.
        /// </summary>
        /// <param name="id">Çalışan ID</param>
        /// <returns>Çalışan DTO</returns>
        Task<EmployeeDto> GetEmployeeByIdAsync(int id);

        /// <summary>
        /// Tüm çalışanları getirir.
        /// </summary>
        /// <returns>DTO listesi</returns>
        Task<List<EmployeeDto>> GetAllEmployeesAsync();

        /// <summary>
        /// Çalışanı siler.
        /// </summary>
        /// <param name="id">Silinecek çalışan ID</param>
        /// <returns>Başarı durumu</returns>
        Task<bool> DeleteEmployeeAsync(int id);

        /// <summary>
        /// Yöneticiler için sabit maaş hesaplar (160 saat x saatlik ücret).
        /// </summary>
        /// <param name="employeeId">Çalışan ID</param>
        /// <param name="hourlyRate">Saatlik ücret</param>
        /// <returns>Aylık maaş</returns>
        Task<decimal> GetManagerSalaryAsync(int employeeId, decimal hourlyRate);

        /// <summary>
        /// Çalışana belirli bir vardiyayı belirli tarihte atar.
        /// </summary>
        /// <param name="employeeId">Çalışan ID</param>
        /// <param name="shiftId">Vardiya ID</param>
        /// <param name="assignedDate">Atama tarihi</param>
        /// <returns>Başarı durumu</returns>
        Task<bool> AssignShiftAsync(int employeeId, int shiftId, DateTime assignedDate);

        /// <summary>
        /// Çalışanın tüm vardiyaları üzerinden toplam saat hesaplar ve maaşı döner.
        /// </summary>
        /// <param name="employeeId">Çalışan ID</param>
        /// <param name="hourlyRate">Saatlik ücret</param>
        /// <returns>Maaş</returns>
        Task<decimal> CalculateSalaryAsync(int employeeId, decimal hourlyRate);

        /// <summary>
        /// 160 saatin üstündeki fazla çalışmaları hesaplar.
        /// </summary>
        /// <param name="employeeId">Çalışan ID</param>
        /// <returns>Fazla mesai (saat)</returns>
        Task<int> TrackOvertimeAsync(int employeeId);

      //  Task<List<Employee>> GetAllWithIncludeAsync(Expression<Func<Employee, bool>> predicate = null, Func<IQueryable<Employee>, IQueryable<Employee>> include = null);

        Task<List<EmployeeDto>> GetAllEmployeesWithDetailsAsync();
    }
}
