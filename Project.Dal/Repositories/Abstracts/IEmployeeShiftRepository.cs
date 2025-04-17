using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IEmployeeShiftRepository:IRepository<EmployeeShift>
    {
        Task<List<EmployeeShift>> GetShiftsByDateAsync(DateTime date); // Belirli bir tarihteki vardiyaları getir
    }
}
