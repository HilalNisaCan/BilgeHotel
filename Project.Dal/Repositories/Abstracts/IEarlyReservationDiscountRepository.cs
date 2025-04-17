using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IEarlyReservationDiscountRepository : IRepository<EarlyReservationDiscount>
    {
        Task<EarlyReservationDiscount> GetDiscountByReservationIdAsync(int reservationId); // Rezervasyon ID'ye göre indirim getir
        Task<List<EarlyReservationDiscount>> GetDiscountsByCustomerIdAsync(int customerId); // Müşterinin tüm erken rezervasyon indirimlerini getir
    }
}

