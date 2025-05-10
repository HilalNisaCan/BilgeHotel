using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DtoClasses;
using Project.BLL.Helpers;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{

    /// <summary>
    /// Yorum (review) işlemlerini yöneten manager sınıfı.
    /// </summary>
    public class ReviewManager : BaseManager<ReviewDto, Review>, IReviewManager
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewManager(
            IReviewRepository reviewRepository,
      
            IMapper mapper)
            : base(reviewRepository, mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni bir yorum ekler. Eklenmeden önce rezervasyonun ve odanın doğruluğu kontrol edilir.
        /// </summary>
        public async Task<bool> AddReviewAsync(ReviewDto dto)
        {
            Review entity = _mapper.Map<Review>(dto);
            await _reviewRepository.AddAsync(entity);
            return true;
        }

        /// <summary>
        /// Belirli bir yorumu onaylar.
        /// </summary>
        public async Task<bool> ApproveReviewAsync(int id)
        {
            Review review = await _reviewRepository.GetByIdAsync(id);
            if (review == null) return false;

            review.IsApproved = true;
            review.ModifiedDate = DateTime.Now;
            await _reviewRepository.UpdateAsync(review);
            return true;
        }


        /// <summary>
        /// Onaylanmamış bir yorumu siler.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            Review? review = await _reviewRepository.GetByIdAsync(id);
            if (review == null) return false;
            if (review.IsApproved) return false;

            await _reviewRepository.RemoveAsync(review);
            return true;
        }

        /// <summary>
        /// Include içeren filtreli tüm yorumları getirir.
        /// </summary>
        public async Task<List<Review>> GetAllWithIncludeAsync(
            Expression<Func<Review, bool>> predicate,
            Func<IQueryable<Review>, IQueryable<Review>> include)
        {
            IEnumerable<Review> result = await _reviewRepository.GetAllWithIncludeAsync(predicate, include);
            return result.ToList();
        }


        /// <summary>
        /// Tüm yorumlar içindeki anonim yorum yüzdesini hesaplar.
        /// </summary>
        public async Task<double> GetAnonymousRateAsync()
        {
            List<Review> allReviews = (await _reviewRepository.GetAllAsync()).ToList();
            return ReviewStatisticsHelper.GetAnonymousRate(allReviews);
        }

        /// <summary>
        /// İsteğe bağlı olarak tüm yorumlar veya sadece bir oda tipi için ortalama puanı getirir.
        /// </summary>
        public async Task<double> GetAverageRatingAsync(RoomType? roomType = null)
        {
            List<Review> allReviews = (await _reviewRepository.GetAllAsync()).ToList();
            return ReviewStatisticsHelper.GetAverageRating(allReviews, roomType);
        }


        /// <summary>
        /// Belirli bir oda tipine ait onaylı yorumların ortalama puanını getirir.
        /// </summary>
        public async Task<double> GetAverageRatingByRoomTypeAsync(RoomType roomType)
        {
            List<Review> reviews = (await _reviewRepository.GetAllAsync(r => r.RoomType == roomType && r.IsApproved)).ToList();
            if (!reviews.Any()) return 0;

            return Math.Round(reviews.Average(r => r.Rating), 1);
        }

        /// <summary>
        /// Onay bekleyen yorum sayısını getirir.
        /// </summary>
        public async Task<int> GetPendingReviewCountAsync()
        {
            List<Review> allReviews = (await _reviewRepository.GetAllAsync()).ToList();
            return ReviewStatisticsHelper.GetPendingReviews(allReviews).Count;
        }

        /// <summary>
        /// Onay bekleyen yorumları DTO olarak getirir.
        /// </summary>
        public async Task<List<ReviewDto>> GetPendingReviewsAsync()
        {
            List<Review> reviews = (await _reviewRepository.GetAllWithIncludeAsync(
                r => !r.IsApproved,
                include: r => r.Include(x => x.User).ThenInclude(u => u.UserProfile)
            )).ToList();

            return _mapper.Map<List<ReviewDto>>(reviews);
        }


        /// <summary>
        /// Belirli bir oda tipine ait onaylı yorumları getirir.
        /// </summary>
        public async Task<List<ReviewDto>> GetReviewsByRoomTypeAsync(RoomType roomType)
        {
            List<Review> reviews = (await _reviewRepository.GetAllAsync(
                r => r.RoomType == roomType && r.IsApproved)).ToList();

            return _mapper.Map<List<ReviewDto>>(reviews);
        }
    }
}
