using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IPaymentRepository:IRepository<Payment>
    {
        Task<List<Payment>> GetPaymentsByUserIdAsync(int userId); // Kullanıcının yaptığı tüm ödemeleri getir
        Task<List<Payment>> GetPaymentsByReservationIdAsync(int reservationId); // Bir rezervasyona ait tüm ödemeleri getir
        Task<decimal> GetTotalPaidAmountByReservationAsync(int reservationId); // Bir rezervasyon için toplam ödenen miktarı getir
   
    }
}

