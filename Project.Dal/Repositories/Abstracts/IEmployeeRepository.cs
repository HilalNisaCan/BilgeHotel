using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IEmployeeRepository:IRepository<Employee>
    {
        Task<Employee> GetEmployeeWithShiftsAsync(int employeeId); // Çalışanı vardiyalarıyla birlikte getir
        Task<List<Employee>> GetEmployeesByPositionAsync(EmployeePosition position); // Belirli bir pozisyondaki çalışanları getir
        Task<List<Employee>> GetAllEmployeesWithUserAsync();
    }
}
