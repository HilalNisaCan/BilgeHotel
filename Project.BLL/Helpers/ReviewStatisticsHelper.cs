using Project.Dal.ContextClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Helpers
{
    public static class ReviewStatisticsHelper
    {
        /// <summary>
        /// Ortalama puanı hesaplar. Tüm yorumlar veya belirli bir oda tipine göre filtrelenmiş yorumlar alınabilir.
        /// </summary>
        public static double GetAverageRating(List<Review> reviews, RoomType? roomType = null)
        {
            if (roomType.HasValue)
                reviews = reviews.Where(r => r.RoomType == roomType.Value).ToList();

            return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
        }

        /// <summary>
        /// Onay bekleyen yorumları döner.
        /// </summary>
        public static List<Review> GetPendingReviews(List<Review> reviews)
        {
            return reviews.Where(r => !r.IsApproved).ToList();
        }

        /// <summary>
        /// Anonim yorumların yüzdesini hesaplar.
        /// </summary>
        public static double GetAnonymousRate(List<Review> reviews)
        {
            if (!reviews.Any()) return 0;

            int anonymousCount = reviews.Count(r => r.IsAnonymous);
            return (double)anonymousCount / reviews.Count * 100;
        }
    }


    /*💡 Neden BLL?
Çünkü bu helper:

Veritabanı işlemi yapmıyor (DAL değil),

Entity üretmiyor (Entities değil),

View’a veri vermiyor (MVC değil),

Tamamen iş mantığına destek oluyor → BLL (Business Logic Layer)

    */

    /* Kullanım Senaryoları:
    Admin panelinde yorumlar listelenirken:

    Ortalama puan: ⭐ 4.3/5

    Onay bekleyen yorum: ❗ 3 adet

    Anonim yorum oranı: 👤 %40
    */


    //"Veri hesaplıyor, analiz yapıyor, veritabanına erişiyorsa → BLL/Helpers
    // Görsel iş, metin biçimlendirme yapıyorsa → MVC/Helpers"

}
