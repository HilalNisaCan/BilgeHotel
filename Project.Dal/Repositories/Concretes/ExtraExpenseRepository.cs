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
    public class ExtraExpenseRepository : BaseRepository<ExtraExpense>, IExtraExpenseRepository
    {
        public ExtraExpenseRepository(MyContext context) : base(context) { }

        public async Task<List<ExtraExpense>> GetExpensesByReservationIdAsync(int reservationId)
        {
            return await _dbSet.Where(ee => ee.ReservationId == reservationId).ToListAsync();
        }

        public async Task<decimal> GetTotalExtraExpensesByReservationIdAsync(int reservationId)
        {
            return await _dbSet.Where(ee => ee.ReservationId == reservationId).SumAsync(ee => ee.UnitPrice);
        }
    }
}
