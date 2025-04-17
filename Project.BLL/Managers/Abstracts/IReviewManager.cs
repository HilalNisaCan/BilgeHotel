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
    /// Yorum yönetimi ile ilgili iş akışlarını tanımlar.
    /// </summary>
    public interface IReviewManager : IManager<ReviewDto, Review>
    {
        /// <summary>
        /// Belirli bir rezervasyona yorum ekler.
        /// </summary>
        Task<bool> AddReviewAsync(ReviewDto dto);

        /// <summary>
        /// Yorumu sistemden siler.
        /// </summary>
        Task<bool> DeleteReviewAsync(int id);

        /// <summary>
        /// Yorumu günceller.
        /// </summary>
        Task<bool> UpdateReviewAsync(ReviewDto dto);

        /// <summary>
        /// Belirli bir odaya ait tüm yorumları getirir.
        /// </summary>
        Task<List<ReviewDto>> GetReviewsByRoomAsync(int roomId);

        /// <summary>
        /// Belirli bir rezervasyona ait yorumu getirir.
        /// </summary>
        Task<ReviewDto> GetReviewByReservationAsync(int reservationId);

        /// <summary>
        /// Belirli bir oda için ortalama puanı hesaplar.
        /// </summary>
        Task<double> GetAverageRatingAsync(int roomId);

        Task<List<Review>> GetAllWithIncludeAsync(Expression<Func<Review, bool>> predicate, Func<IQueryable<Review>, IQueryable<Review>> include);

        
    }
}
