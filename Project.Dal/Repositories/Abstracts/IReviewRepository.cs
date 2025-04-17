using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IReviewRepository:IRepository<Review>
    {
        Task<List<Review>> GetReviewsByUserIdAsync(int userId); // Kullanıcının yaptığı yorumları getir
        Task<List<Review>> GetReviewsByReservationIdAsync(int reservationId); // Bir rezervasyon için bırakılan yorumları getir
        Task<double> GetAverageRatingAsync(); // Otelin ortalama puanını getir
        Task<List<Review>> GetTopRatedReviewsAsync(int topCount); // En yüksek puanlı yorumları getir
    }
}
