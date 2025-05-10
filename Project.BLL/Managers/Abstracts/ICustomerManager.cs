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
    /// <summary>
    /// Müşteri işlemlerini yöneten arayüz.
    /// </summary>
    public interface ICustomerManager : IManager<CustomerDto, Customer>
    {
        /// <summary>
        /// Müşteri yoksa oluşturur, varsa getirir.
        /// </summary>
        Task<CustomerDto> GetOrCreateCustomerAsync(int userId, string identityNumber, string firstName, string lastName, int birthYear);

        /// <summary>
        /// Müşteriye ait rezervasyonları getirir.
        /// </summary>
        Task<List<ReservationDto>> GetCustomerReservationsAsync(int customerId);

        /// <summary>
        /// Müşteri kimliğini doğrular (TCKN, ad, soyad, doğum yılı).
        /// </summary>
        Task<bool> VerifyCustomerIdentityAsync(KimlikBilgisiDto dto);

        /// <summary>
        /// UserId üzerinden müşteri kaydını getirir.
        /// </summary>
        Task<CustomerDto?> GetByUserIdAsync(int userId);

        /// <summary>
        /// Tüm müşterilerin performans raporlarını getirir.
        /// </summary>
        Task<List<CustomerReportDto>> GetAllCustomerReportsAsync();

        /// <summary>
        /// Yeni müşteri kaydı oluşturur.
        /// </summary>
        Task<int> AddAsync(CustomerDto dto);

        Task<CustomerReportDto?> GetCustomerReportWithReservationsAsync(int id);

        Task<bool> DeleteAsync(int id);
    }
}

