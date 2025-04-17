using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IExtraExpenseRepository:IRepository<ExtraExpense>
    {
        Task<List<ExtraExpense>> GetExpensesByReservationIdAsync(int reservationId); // Bir rezervasyona ait ekstra harcamaları getir
        Task<decimal> GetTotalExtraExpensesByReservationIdAsync(int userId); // Kullanıcının toplam ekstra harcamalarını getir
    }
}
