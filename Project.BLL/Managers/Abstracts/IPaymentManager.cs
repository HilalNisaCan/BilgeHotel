using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    public interface IPaymentManager : IManager<PaymentDto, Payment>
    {

        /// <summary>
        /// Belirtilen rezervasyona ait toplam borcu hesaplar. 
        /// Ödemeler + ekstra harcamalar + siparişler toplamı alınır.
        /// </summary>
        Task<decimal> CalculateTotalBillAsync(int reservationId);

        /// <summary>
        /// Belirtilen rezervasyona ödeme işlemi gerçekleştirir.
        /// Kalan borçtan fazla ödeme yapılamaz. Tamamsa Completed, eksikse Pending statüsü verilir.
        /// </summary>
        Task<bool> ProcessPaymentAsync(int reservationId, decimal amount, PaymentMethod method);

        /// <summary>
        /// Ödeme durumu manuel olarak güncellenir (örneğin Admin müdahalesi).
        /// </summary>
        Task UpdatePaymentStatusAsync(int paymentId, PaymentStatus status);

        /// <summary>
        /// Belirtilen ödemeye iade işlemi uygulanır. Açıklama eklenir.
        /// </summary>
        Task<bool> RefundPaymentAsync(int paymentId, string reason);

        /// <summary>
        /// Belirtilen rezervasyonun kalan borç tutarını hesaplar.
        /// </summary>
        Task<decimal> GetRemainingBalanceAsync(int reservationId);

        Task<int> CreateAndReturnIdAsync(PaymentDto dto);


    }
}
