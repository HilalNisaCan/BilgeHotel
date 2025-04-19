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
        /// Belirli bir rezervasyona yorum ekler.
        /// </summary>
        Task<bool> AddReviewAsync(ReviewDto dto);

        Task<List<ReviewDto>> GetPendingReviewsAsync();

        Task<bool> ApproveReviewAsync(int id);

        Task<bool> DeleteAsync(int id);

        Task<List<Review>> GetAllWithIncludeAsync(Expression<Func<Review, bool>> predicate, Func<IQueryable<Review>, IQueryable<Review>> include);

        Task<double> GetAverageRatingByRoomTypeAsync(RoomType roomType);

        Task<List<ReviewDto>> GetReviewsByRoomTypeAsync(RoomType roomType);

        Task<double> GetAverageRatingAsync(RoomType? roomType = null);

        Task<int> GetPendingReviewCountAsync();

        Task<double> GetAnonymousRateAsync();


    }
}
