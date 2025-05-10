using Project.BLL.DtoClasses;
using Project.Entities.Enums;
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
    /// Yorum yönetimi ile ilgili iş akışlarını tanımlar.
    /// </summary>
    public interface IReviewManager : IManager<ReviewDto, Review>
    {
        /// <summary>
        /// Belirli bir rezervasyona ait yorum ekler.
        /// </summary>
        /// <param name="dto">Yorum DTO’su</param>
        /// <returns>Yorum başarıyla eklendiyse true</returns>
        /// 
        /// <remarks>
        /// 📌 Not: Kullanıcı yorum sayfasından gönderim yaptığında çağrılır.
        /// </remarks>
        Task<bool> AddReviewAsync(ReviewDto dto);

        /// <summary>
        /// Onay bekleyen tüm yorumları listeler.
        /// </summary>
        /// <returns>DTO listesi olarak bekleyen yorumlar</returns>
        /// 
        /// <remarks>
        /// 📌 Not: Admin panelde yorum moderasyonu ekranında kullanılır.
        /// </remarks>
        Task<List<ReviewDto>> GetPendingReviewsAsync();

        /// <summary>
        /// Belirtilen ID’ye ait yorumu onaylar.
        /// </summary>
        /// <param name="id">Yorum ID</param>
        /// <returns>Başarılıysa true</returns>
        Task<bool> ApproveReviewAsync(int id);

        /// <summary>
        /// Yorumu siler.
        /// </summary>
        /// <param name="id">Silinecek yorumun ID’si</param>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Tüm yorumları ilişkili verilerle birlikte getirir.
        /// </summary>
        /// <param name="predicate">Filtreleme şartı</param>
        /// <param name="include">Include işlemleri</param>
        Task<List<Review>> GetAllWithIncludeAsync(
            Expression<Func<Review, bool>> predicate,
            Func<IQueryable<Review>, IQueryable<Review>> include);

        /// <summary>
        /// Belirli oda tipine ait ortalama puanı getirir.
        /// </summary>
        /// <param name="roomType">Oda tipi</param>
        Task<double> GetAverageRatingByRoomTypeAsync(RoomType roomType);

        /// <summary>
        /// Belirli oda tipine ait tüm yorumları getirir.
        /// </summary>
        /// <param name="roomType">Oda tipi</param>
        Task<List<ReviewDto>> GetReviewsByRoomTypeAsync(RoomType roomType);

        /// <summary>
        /// Tüm yorumlar veya seçili oda tipi için ortalama puanı hesaplar.
        /// </summary>
        /// <param name="roomType">İsteğe bağlı oda tipi</param>
        Task<double> GetAverageRatingAsync(RoomType? roomType = null);

        /// <summary>
        /// Onay bekleyen yorum sayısını döner.
        /// </summary>
        Task<int> GetPendingReviewCountAsync();

        /// <summary>
        /// Tüm yorumlar içerisindeki anonim yorum yüzdesini hesaplar.
        /// </summary>
        Task<double> GetAnonymousRateAsync();


    }
}
