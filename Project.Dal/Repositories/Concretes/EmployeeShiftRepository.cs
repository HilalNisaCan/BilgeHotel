using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class EmployeeShiftRepository : BaseRepository<EmployeeShift>, IEmployeeShiftRepository
    {
        public EmployeeShiftRepository(MyContext context) : base(context)
        {
        }
        public async Task<List<EmployeeShift>> GetShiftsByDateAsync(DateTime date)
        {
            return await _dbSet.Where(es => es.ShiftDate.Date == date.Date).ToListAsync();
        }
    }
}
