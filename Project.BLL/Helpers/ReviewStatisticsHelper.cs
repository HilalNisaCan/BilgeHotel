using Project.Dal.ContextClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Helpers
{
    /// <summary>
    /// ReviewStatisticsHelper sınıfı, müşteri yorumlarıyla ilgili istatistiksel analizleri sağlar.
    /// Yorumların değerlendirilmesi, filtrelenmesi ve istatistiksel özetlerinin çıkarılması için yardımcı olur.
    /// </summary>
    public static class ReviewStatisticsHelper
    {
        /// <summary>
        /// Belirli bir odaya ya da tüm otel geneline ait ortalama puanı hesaplar.
        /// </summary>
        /// <param name="context">Veritabanı bağlamı</param>
        /// <param name="roomId">İsteğe bağlı olarak belirli bir oda ID’si girilebilir</param>
        /// <returns>1 ile 5 arasında ortalama puan değeri</returns>
        public static double GetAverageRating(MyContext context, int? roomId = null)
        {
            var query = context.Reviews.AsQueryable();

            if (roomId.HasValue)
                query = query.Where(r => r.RoomId == roomId.Value);

            return query.Any() ? query.Average(r => r.Rating) : 0;
        }

        /// <summary>
        /// Onay bekleyen (IsApproved == false) tüm yorumları getirir.
        /// Yorum denetleme ve yönetici kontrol ekranlarında kullanılır.
        /// </summary>
        /// <param name="context">Veritabanı bağlamı</param>
        /// <returns>Onay bekleyen yorum listesi</returns>
        public static List<Review> GetPendingReviews(MyContext context)
        {
            return context.Reviews.Where(r => !r.IsApproved).ToList();
        }

        /// <summary>
        /// Sistem genelinde yapılan yorumlar içerisinde anonim olanların yüzdesini döner.
        /// Kullanıcı davranışlarını analiz etmek için faydalıdır.
        /// </summary>
        /// <param name="context">Veritabanı bağlamı</param>
        /// <returns>Anonim yorumların yüzdesi (0–100 arası)</returns>
        public static double GetAnonymousRate(MyContext context)
        {
            var total = context.Reviews.Count();
            if (total == 0) return 0;

            var anonymous = context.Reviews.Count(r => r.IsAnonymous);
            return (double)anonymous / total * 100;
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
