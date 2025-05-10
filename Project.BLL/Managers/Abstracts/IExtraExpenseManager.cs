using Project.BLL.DtoClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    /// <summary>
    /// Oteldeki ekstra masrafların yönetimini sağlayan arayüz.
    /// </summary>
    public interface IExtraExpenseManager : IManager<ExtraExpenseDto, ExtraExpense>
    {


        /// <summary>
        /// Belirli rezervasyona ait tüm masrafları getirir.
        /// </summary>
        Task<List<ExtraExpenseDto>> GetExpensesByReservationAsync(int reservationId);

        /// <summary>
        /// Yeni masraf kaydı ekler.
        /// </summary>
        Task AddAsync(ExtraExpenseDto dto);
    }
}
