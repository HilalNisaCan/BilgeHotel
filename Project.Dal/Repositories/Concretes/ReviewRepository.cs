using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
    
        public ReviewRepository(MyContext context) : base(context)
        {
        }

        public async Task<List<Review>> GetReviewsByUserIdAsync(int userId)
        {
            return await _dbSet.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByReservationIdAsync(int reservationId)
        {
            return await _dbSet.Where(r => r.ReservationId == reservationId).ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync()
        {
            return await _dbSet.AverageAsync(r => r.Rating);
        }

        public async Task<List<Review>> GetTopRatedReviewsAsync(int topCount)
        {
            return await _dbSet.OrderByDescending(r => r.Rating).Take(topCount).ToListAsync();
        }
    }
}
