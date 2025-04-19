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
        /// Yeni bir rezervasyon oluşturur. Uygunluk kontrolü yapılır, rezervasyon fiyatı kampanya ve erken rezervasyon indirimi dikkate alınarak hesaplanır.
        /// </summary>
        /// <param name="customerId">Rezervasyonu yapacak müşteri ID'si</param>
        /// <param name="roomId">Rezerve edilecek oda ID'si</param>
        /// <param name="startDate">Konaklama başlangıç tarihi</param>
        /// <param name="duration">Konaklama süresi (gün)</param>
        /// <param name="package">Seçilen rezervasyon paketi (örneğin: Tam Pansiyon)</param>
        /// <returns>Rezervasyon başarıyla yapılırsa true, aksi halde false</returns>
        Task<bool> MakeReservationAsync(int customerId, int roomId, DateTime startDate, int duration, ReservationPackage package);

        /// <summary>
        /// Belirli bir odanın belirtilen tarihlerde uygun olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="roomId">Kontrol edilecek oda ID'si</param>
        /// <param name="startDate">İstenen başlangıç tarihi</param>
        /// <param name="duration">Konaklama süresi (gün)</param>
        /// <returns>Uygunsa true, doluysa false</returns>
        Task<bool> CheckAvailabilityAsync(int roomId, DateTime startDate, int duration);

        /// <summary>
        /// Belirtilen rezervasyonu iptal eder. Odanın durumu 'Available' olarak güncellenir.
        /// </summary>
        /// <param name="reservationId">İptal edilecek rezervasyonun ID'si</param>
        Task CancelReservationAsync(int reservationId);

        /// <summary>
        /// Rezervasyon süresi ve oda fiyatına göre toplam ücreti hesaplar.
        /// </summary>
        /// <param name="reservationId">Fiyatı hesaplanacak rezervasyonun ID'si</param>
        /// <returns>Toplam fiyat</returns>
        Task<decimal> CalculateTotalPriceAsync(int reservationId);

        /// <summary>
        /// Belirli bir müşterinin geçmiş ve mevcut tüm rezervasyonlarını getirir.
        /// </summary>
        /// <param name="customerId">Müşteri ID</param>
        /// <returns>Rezervasyon DTO listesi</returns>
        Task<List<ReservationDto>> GetCustomerReservationsAsync(int customerId);

        Task<int> CreateAndReturnIdAsync(int customerId, int roomId, DateTime checkIn, int duration, ReservationPackage package, decimal totalPrice);

        Task<int> CreateAndReturnIdAsync(int customerId, int userId, int roomId, DateTime checkIn, int duration, ReservationPackage package, decimal totalPrice);

        Task<ReservationDto> GetByIdWithRoomAsync(int reservationId);
        Task<bool> UpdateAfterPaymentAsync(int reservationId, int paymentId);


        Task<List<ReservationDto>> GetAllWithRoomAndCustomerAsync();

        Task<ReservationDto> GetByIdWithRoomAndCustomerAsync(int reservationId);

        Task<bool> UpdateStatusAsync(int reservationId, ReservationStatus status);

        Task<List<Reservation>> GetAllWithIncludeAsync(
    Expression<Func<Reservation, bool>> predicate,
    Func<IQueryable<Reservation>, IQueryable<Reservation>> include);

        Task<int> AddAsync(ReservationDto reservationDto);

        Task<List<ReservationDto>> GetTodayCheckOutsAsync();

         Task<ReservationDto> GetWithIncludeAsync(int reservationId);

        Task<bool> CompleteReservationAsync(int reservationId);

       
    }
}
