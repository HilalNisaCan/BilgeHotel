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
    public class EarlyReservationDiscountRepository : BaseRepository<EarlyReservationDiscount>, IEarlyReservationDiscountRepository
    {
        public EarlyReservationDiscountRepository(MyContext context) : base(context)
        {
        }

        // Rezervasyon ID'ye göre erken rezervasyon indirimini getir
        public async Task<EarlyReservationDiscount> GetDiscountByReservationIdAsync(int reservationId)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbSet
                .FirstOrDefaultAsync(e => e.ReservationId == reservationId);
#pragma warning restore CS8603 // Possible null reference return.
        }

        // Müşterinin aldığı tüm erken rezervasyon indirimlerini getir
        public async Task<List<EarlyReservationDiscount>> GetDiscountsByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Where(e => e.CustomerId == customerId)
                .ToListAsync();
        }
    }
}
