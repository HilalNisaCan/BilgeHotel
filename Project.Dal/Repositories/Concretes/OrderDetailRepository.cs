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
    public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(MyContext context) : base(context)
        {
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _dbSet.Where(od => od.OrderId == orderId).ToListAsync();
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByProductIdAsync(int productId)
        {
            return await _dbSet.Where(od => od.ProductId == productId).ToListAsync();
        }
    }
}
