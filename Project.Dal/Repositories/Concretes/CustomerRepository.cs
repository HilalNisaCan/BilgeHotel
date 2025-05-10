using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(MyContext context) : base(context)
        {

        }

        public new async Task<Customer> GetByIdAsync(int customerId)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbSet.FirstOrDefaultAsync(c => c.Id == customerId);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<Customer> GetByIdentityNumberAsync(string identityNumber)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Customers
                .Include(c => c.User)
                .ThenInclude(u => u.UserProfile)
                .FirstOrDefaultAsync(c => c.IdentityNumber == identityNumber);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<Customer?> GetByIdWithUserAndReservationsAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.User)
                    .ThenInclude(u => u.UserProfile)
                .Include(c => c.Reservations)
                    .ThenInclude(r => r.Room) // ⬅ Bu satırı mutlaka ekle
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> GetByUserIdAsync(int userId)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Customers
                .Include(c => c.User)
                .ThenInclude(u => u.UserProfile)
                .FirstOrDefaultAsync(c => c.UserId == userId);
#pragma warning restore CS8603 // Possible null reference return.
        }
        // Müşteriyi rezervasyonlarıyla birlikte getir
        public async Task<Customer> GetCustomerWithReservationsAsync(int customerId)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbSet
                .Include(c => c.Reservations)
                .FirstOrDefaultAsync(predicate: c => c.Id == customerId);
#pragma warning restore CS8603 // Possible null reference return.
        }
        // Belirli sadakat puanı üzerindeki müşterileri getir
        public async Task<List<Customer>> GetHighLoyaltyCustomersAsync(int minPoints)
        {
            return await _dbSet
                .Where(c => c.LoyaltyPoints >= minPoints)
                .ToListAsync();
        }
        public async Task<bool> SoftDeleteAsync(int id)
        {
            Customer? customer = await GetByIdAsync(id);
            if (customer == null)
                return false;

            customer.Status = Entities.Enums.DataStatus.Deleted;
            customer.DeletedDate = DateTime.Now;

            await UpdateAsync(customer);
            return true;
        }

        public async Task<List<Customer>> WhereAsync(Expression<Func<Customer, bool>> filter)
        {
            return await _dbSet.Where(filter).ToListAsync();
        }
    }
}
