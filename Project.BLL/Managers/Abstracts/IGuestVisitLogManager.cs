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
    /// Misafir ziyaret kayıtlarıyla ilgili iş kurallarını tanımlar.
    /// </summary>
    public interface IGuestVisitLogManager : IManager<GuestVisitLogDto, GuestVisitLog>
    {
        /// <summary>
        /// Müşteri için giriş kaydı oluşturur (Oda boş olmalı ve müşteri 24 saat içinde çıkış yapmamış olmalı).
        /// </summary>
        Task<bool> RegisterGuestEntryAsync(int customerId, int roomId);

        /// <summary>
        /// Müşteri için çıkış kaydı oluşturur (aktif giriş kaydı bulunmalı).
        /// </summary>
        Task<bool> RegisterGuestExitAsync(int customerId);

        /// <summary>
        /// Müşterinin tüm giriş-çıkış geçmişini listeler.
        /// </summary>
        Task<List<GuestVisitLogDto>> GetGuestVisitHistoryAsync(int customerId);
    }
}
