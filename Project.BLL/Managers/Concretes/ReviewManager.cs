using AutoMapper;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
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
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public ReviewManager(
            IReviewRepository reviewRepository,
            IReservationRepository reservationRepository,
            IRoomRepository roomRepository,
            IMapper mapper)
            : base(reviewRepository, mapper)
        {
            _reviewRepository = reviewRepository;
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni bir yorum ekler. Eklenmeden önce rezervasyonun ve odanın doğruluğu kontrol edilir.
        /// </summary>
        public async Task<bool> AddReviewAsync(ReviewDto dto)
        {
            var reservation = await _reservationRepository.GetByIdAsync(dto.ReservationId);
            if (reservation == null || reservation.RoomId != dto.RoomId || reservation.Customer.UserId != dto.UserId)
                return false;

            var room = await _roomRepository.GetByIdAsync(dto.RoomId);
            if (room == null)
                return false;

            var entity = _mapper.Map<Review>(dto);
            await _reviewRepository.AddAsync(entity);
            return true;
        }

        /// <summary>
        /// Yorumu sistemden siler.
        /// </summary>
        public async Task<bool> DeleteReviewAsync(int id)
        {
            var entity = await _reviewRepository.GetByIdAsync(id);
            if (entity == null) return false;

            await _reviewRepository.RemoveAsync(entity);
            return true;
        }

        /// <summary>
        /// Mevcut yorumu günceller.
        /// </summary>
        public async Task<bool> UpdateReviewAsync(ReviewDto dto)
        {
            var entity = await _reviewRepository.GetByIdAsync(dto.Id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _reviewRepository.UpdateAsync(entity);
            return true;
        }

        /// <summary>
        /// Belirli bir odaya ait tüm onaylı yorumları getirir.
        /// </summary>
        public async Task<List<ReviewDto>> GetReviewsByRoomAsync(int roomId)
        {
            var list = await _reviewRepository.GetAllAsync(x => x.RoomId == roomId && x.IsApproved);
            return _mapper.Map<List<ReviewDto>>(list);
        }

        /// <summary>
        /// Belirli bir rezervasyona ait onaylı yorumu getirir.
        /// </summary>
        public async Task<ReviewDto> GetReviewByReservationAsync(int reservationId)
        {
            var entity = await _reviewRepository.FirstOrDefaultAsync(x => x.ReservationId == reservationId && x.IsApproved);
            return _mapper.Map<ReviewDto>(entity);
        }

        /// <summary>
        /// Belirli bir odaya ait onaylı yorumların ortalama puanını hesaplar.
        /// </summary>
        public async Task<double> GetAverageRatingAsync(int roomId)
        {
            var list = await _reviewRepository.GetAllAsync(x => x.RoomId == roomId && x.IsApproved);
            if (!list.Any()) return 0;

            return list.Average(x => x.Rating);
        }

        public async Task<List<Review>> GetAllWithIncludeAsync(Expression<Func<Review, bool>> predicate, Func<IQueryable<Review>, IQueryable<Review>> include)
        {
            IEnumerable<Review> result = await _reviewRepository.GetAllWithIncludeAsync(predicate, include);
            return result.ToList();
        }
    }
}
