using Project.BLL.DtoClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    public interface ICustomerManager:IManager<CustomerDto,Customer>
    {
        /// <summary>
        /// Müşteri bilgilerini getirir.
        /// </summary>
        Task<CustomerDto> GetCustomerByIdAsync(int customerId);

        /// <summary>
        /// Müşteri yoksa oluşturur.
        /// </summary>
        Task<CustomerDto> GetOrCreateCustomerAsync(int userId, string identityNumber, string firstName, string lastName, int birthYear);

        /// <summary>
        /// Müşterinin rezervasyon geçmişini getirir.
        /// </summary>
        Task<List<ReservationDto>> GetCustomerReservationsAsync(int customerId);

        /// <summary>
        /// Müşterinin sadakat puanını getirir.
        /// </summary>
        Task<decimal> GetLoyaltyPointsAsync(int customerId);

        /// <summary>
        /// Müşterinin sadakat puanını günceller.
        /// </summary>
        Task UpdateLoyaltyPointsAsync(int customerId, decimal points);

        /// <summary>
        /// Müşterinin kimlik bilgilerini doğrular.
        /// </summary>
        Task<bool> VerifyCustomerIdentityAsync(KimlikBilgisiDto dto);

        Task<CustomerDto?> GetByUserIdAsync(int userId);

        /// <summary>
        /// Tüm müşterilerin rezervasyon, harcama ve sadakat puanı gibi performans bilgilerini getirir.
        /// </summary>
        /// <returns>Müşteri bazlı rapor listesi</returns>
        Task<List<CustomerReportDto>> GetAllCustomerReportsAsync();

        Task<CustomerReportDto?> GetCustomerReportByIdAsync(int id);

        Task<int> AddAsync(CustomerDto dto);
    }
}

