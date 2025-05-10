using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Project.Dal.Repositories.Abstracts
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetCustomerWithReservationsAsync(int customerId); // Müşteriyi rezervasyonlarıyla getir
        Task<List<Customer>> GetHighLoyaltyCustomersAsync(int minPoints); // Belirli sadakat puanı üzerindeki müşterileri getir
        new Task<Customer> GetByIdAsync(int customerId);
        Task<List<Customer>> WhereAsync(Expression<Func<Customer, bool>> filter);
        new Task UpdateAsync(Customer customer);
        /// <summary>
        /// UserId üzerinden müşteri bulur.
        /// </summary>
        Task<Customer> GetByUserIdAsync(int userId);

        /// <summary>
        /// TC kimlik numarasına göre müşteri bulur.
        /// </summary>
        Task<Customer> GetByIdentityNumberAsync(string identityNumber);

        Task<Customer?> GetByIdWithUserAndReservationsAsync(int id);
        Task<bool> SoftDeleteAsync(int id);

    }
}
