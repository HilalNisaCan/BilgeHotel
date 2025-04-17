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
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(MyContext context) : base(context)
        {
        }
        public async Task<List<Payment>> GetPaymentsByUserIdAsync(int userId)
        {
            return await _dbSet.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByReservationIdAsync(int reservationId)
        {
            return await _dbSet.Where(p => p.ReservationId == reservationId).ToListAsync();
        }

        public async Task<decimal> GetTotalPaidAmountByReservationAsync(int reservationId)
        {
            return await _dbSet.Where(p => p.ReservationId == reservationId).SumAsync(p => p.TotalAmount);
        }
    }
}
