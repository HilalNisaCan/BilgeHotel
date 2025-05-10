using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    public interface IReservationManager : IManager<ReservationDto, Reservation>
    {
        /// <summary>
        /// Belirli bir müşterinin geçmiş ve mevcut tüm rezervasyonlarını getirir.
        /// </summary>
        /// <param name="customerId">Müşteri ID</param>
        /// <returns>Rezervasyon DTO listesi</returns>
        /// 
        /// <remarks>
        /// 📌 Not: Kullanıcı panelinde “rezervasyon geçmişi” gibi ekranlarda kullanılır.
        /// </remarks>
        Task<List<ReservationDto>> GetCustomerReservationsAsync(int customerId);

        /// <summary>
        /// Yeni rezervasyon oluşturur ve ID değerini döner.
        /// </summary>
        /// <remarks>
        /// 📌 Not: Customer tablosu üzerinden yapılan işlemlerde çağrılır.
        /// </remarks>
        Task<int> CreateAndReturnIdAsync(
            int customerId,
            int roomId,
            DateTime checkIn,
            int duration,
            ReservationPackage package,
            decimal totalPrice);

        /// <summary>
        /// Yeni rezervasyon oluşturur ve ID değerini döner (User bilgisiyle birlikte).
        /// </summary>
        /// <remarks>
        /// 📌 Not: Kullanıcı sistemde kayıtlıysa hem user hem customer ID ile çalışılır.
        /// </remarks>
        Task<int> CreateAndReturnIdAsync(
          int customerId,
          int userId,
          int roomId,
          DateTime checkIn,
          int duration,
          ReservationPackage package,
          decimal totalPrice,
          int numberOfGuests,
          int? campaignId = null,
          decimal discountRate = 0,
         string currencyCode = "TRY");

        /// <summary>
        /// Tüm rezervasyonları oda ve müşteri bilgisiyle birlikte getirir.
        /// </summary>
        /// <returns>Rezervasyon listesi</returns>
        /// 
        /// <remarks>
        /// 📌 Not: Admin panelde toplu rezervasyon listelemede kullanılır.
        /// </remarks>
        Task<List<ReservationDto>> GetAllWithRoomAndCustomerAsync();

        /// <summary>
        /// Belirli rezervasyonun oda ve müşteri bilgisiyle detayını getirir.
        /// </summary>
        /// <param name="reservationId">Rezervasyon ID</param>
        Task<ReservationDto> GetByIdWithRoomAndCustomerAsync(int reservationId);

        /// <summary>
        /// Rezervasyonun durumunu günceller (örn. Onaylandı, İptal edildi).
        /// </summary>
        Task<bool> UpdateStatusAsync(int reservationId, ReservationStatus status);

        /// <summary>
        /// Include içeren tüm rezervasyonları filtreleyerek getirir.
        /// </summary>
        Task<List<Reservation>> GetAllWithIncludeAsync(
            Expression<Func<Reservation, bool>> predicate,
            Func<IQueryable<Reservation>, IQueryable<Reservation>> include);

        /// <summary>
        /// DTO ile rezervasyon oluşturur ve ID döner.
        /// </summary>
        Task<int> AddAsync(ReservationDto reservationDto);

        /// <summary>
        /// Rezervasyon bilgilerini ilişkili oda ve müşteri bilgisiyle birlikte getirir.
        /// </summary>
        /// <param name="reservationId">Rezervasyon ID</param>
        Task<ReservationDto> GetWithIncludeAsync(int reservationId);

        /// <summary>
        /// Rezervasyonu tamamlama (örneğin: ödeme sonrası bitirme) işlemi.
        /// </summary>
        /// <param name="reservationId">İlgili rezervasyonun ID’si</param>
        Task<bool> CompleteReservationAsync(int reservationId);

        Task<bool> DeleteAsync(int id);

    }
}
