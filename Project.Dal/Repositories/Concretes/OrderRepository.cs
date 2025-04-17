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
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(MyContext context) : base(context)
        {
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _dbSet.Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByReservationIdAsync(int reservationId)
        {
            return await _dbSet.Where(o => o.ReservationId == reservationId).ToListAsync();
        }
    }
}
