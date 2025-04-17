using Project.BLL.DtoClasses;
using Project.BLL.Managers.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    /// <summary>
    /// Erken rezervasyon indirimlerinin hesaplandığı ve uygulandığı işlemleri tanımlar.
    /// </summary>
    public interface IEarlyReservationDiscountManager : IManager<EarlyReservationDiscountDto, EarlyReservationDiscount>
    {
        /// <summary>
        /// Verilen kriterlere göre toplam indirim hesaplar.
        /// </summary>
        Task<decimal> CalculateDiscountAsync(int customerId, DateTime reservationDate, DateTime checkInDate, decimal basePrice, ReservationPackage package);

        /// <summary>
        /// Belirli bir rezervasyona uygun indirimi hesaplayıp toplam fiyata uygular.
        /// </summary>
        Task<bool> ApplyDiscountToReservationAsync(int reservationId);
    }
}
