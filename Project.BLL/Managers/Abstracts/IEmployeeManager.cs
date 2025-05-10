using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    /// <summary>
    /// Çalışan işlemlerini yöneten arayüz.
    /// </summary>
    public interface IEmployeeManager : IManager<EmployeeDto, Employee>
    {
        /// <summary>
        /// Yeni çalışan oluşturur.
        /// </summary>
        Task<int> AddEmployeeAsync(EmployeeDto dto);

        /// <summary>
        /// Çalışan bilgilerini günceller.
        /// </summary>
        Task<bool> UpdateEmployeeAsync(EmployeeDto dto);

        /// <summary>
        /// ID'ye göre çalışan getirir.
        /// </summary>
        Task<EmployeeDto> GetEmployeeByIdAsync(int id);

        /// <summary>
        /// Tüm çalışanları getirir.
        /// </summary>
        Task<List<EmployeeDto>> GetAllEmployeesAsync();

        /// <summary>
        /// Çalışanı siler.
        /// </summary>
        Task<bool> DeleteEmployeeAsync(int id);

        /// <summary>
        /// Çalışana vardiya ataması yapar.
        /// </summary>
        Task<bool> AssignShiftAsync(int employeeId, int shiftId, DateTime assignedDate);

        /// <summary>
        /// Çalışanın toplam çalışma süresine göre maaş hesaplar.
        /// </summary>
        Task<decimal> CalculateSalaryAsync(int employeeId, decimal hourlyRate);

        /// <summary>
        /// Çalışanları tüm detaylarıyla getirir (pozisyon, iletişim vs.).
        /// </summary>
        Task<List<EmployeeDto>> GetAllEmployeesWithDetailsAsync();

        /// <summary>
        /// Pozisyona göre çalışanları getirir.
        /// </summary>
        Task<List<EmployeeDto>> GetByPositionAsync(EmployeePosition position);

        /// <summary>
        /// Birden fazla pozisyona göre çalışanları getirir.
        /// </summary>
        Task<List<EmployeeDto>> GetByPositionsAsync(EmployeePosition[] positions);
    }
}
