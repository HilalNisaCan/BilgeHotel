using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    /*“DashboardController, yöneticilere sistemin genel durumu hakkında özet bilgiler sunmak amacıyla tasarlanmıştır.
Bu controller, müşteri, oda ve çalışan sayısı gibi temel metrikleri ViewBag ile View’a iletir.
Ayrıca sistemdeki ortalama kullanıcı puanı, onay bekleyen yorum sayısı ve anonim yorum oranı gibi kullanıcı geri bildirimi verileri de yer alır.
Günlük rezervasyon takibi için, bugüne ait rezervasyonlar StartDate filtrelemesiyle çekilir ve listelenir.
Böylece yöneticiler, otelin doluluk durumu, hizmet kalitesi ve müşteri etkileşimi gibi konularda tek bakışta bilgi sahibi olabilir.”
    
    Tüm veriler async servis çağrılarıyla çekiliyor (performans odaklı).

ViewBag ile sade veri aktarımı yapılmış; bu View'da grafikleştirme veya görsel kart yapıları için esnek yapı sağlar.

Günlük rezervasyonlar için StartDate.Date == DateTime.Today filtresi ile LINQ kullanımı örneklenmiş.

ReviewManager üzerinden ortalama puan ve anonim oran gibi istatistiksel veriler çekilerek kullanıcı deneyimi analiz ediliyor.*/



    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly ICustomerManager _customerManager;
        private readonly IRoomManager _roomManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IReservationManager _reservationManager;
        private readonly IPaymentManager _paymentManager;
        private readonly IReviewManager _reviewManager;

        public DashboardController(
            ICustomerManager customerManager,
            IRoomManager roomManager,
            IEmployeeManager employeeManager,
            IReservationManager reservationManager,
            IReviewManager reviewManager,
            IPaymentManager paymentManager)
        {
            _customerManager = customerManager;
            _roomManager = roomManager;
            _employeeManager = employeeManager;
            _reservationManager = reservationManager;
            _reviewManager = reviewManager;
            _paymentManager = paymentManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalCustomers = (await _customerManager.GetAllAsync()).Count;
            ViewBag.TotalRooms = (await _roomManager.GetAllAsync()).Count;
            ViewBag.TotalEmployees = (await _employeeManager.GetAllAsync()).Count;

            ViewBag.AverageRating = await _reviewManager.GetAverageRatingAsync();
            ViewBag.PendingReviewCount = await _reviewManager.GetPendingReviewCountAsync();
            ViewBag.AnonymousRate = await _reviewManager.GetAnonymousRateAsync();
            ViewBag.TotalRevenue = (await _paymentManager.GetAllAsync()).Sum(x => x.TotalAmount);

            // ✅ Bugünkü rezervasyon sayısı için predicate filtreli çekiyoruz
            DateTime today = DateTime.Today;

            List<Project.Entities.Models.Reservation> todayReservations = await _reservationManager.GetAllWithIncludeAsync(
            r => r.StartDate.Date == today,
            include: q => q
            );


            ViewBag.TodayReservations = todayReservations.Count();

            return View();
        }
    }
}
