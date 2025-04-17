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
        /// Yeni bir ekstra masraf ekler.
        /// </summary>
        Task<bool> AddExpenseAsync(ExtraExpenseDto dto);

        /// <summary>
        /// Var olan masraf kaydını günceller.
        /// </summary>
        Task<bool> UpdateExpenseAsync(ExtraExpenseDto dto);

        /// <summary>
        /// Masraf kaydını siler.
        /// </summary>
        Task<bool> DeleteExpenseAsync(int id);

        /// <summary>
        /// Belirli bir rezervasyona ait tüm masrafları getirir.
        /// </summary>
        Task<List<ExtraExpenseDto>> GetExpensesByReservationAsync(int reservationId);

        /// <summary>
        /// Belirli bir masraf kaydını getirir.
        /// </summary>
        Task<ExtraExpenseDto> GetExpenseByIdAsync(int id);

        /// <summary>
        /// Masrafın durumunu günceller (örn. Ödendi, İptal edildi).
        /// </summary>
        Task<bool> UpdateExpenseStatusAsync(int expenseId, string status);

        Task AddAsync(ExtraExpenseDto dto);
    }
}
