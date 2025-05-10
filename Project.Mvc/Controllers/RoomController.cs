using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.Models.PureVm.ResponseModel.Review;
using Project.MvcUI.Models.PureVm.ResponseModel.Room;
using Project.MvcUI.Services;
using System.Security.Claims;


namespace Project.MvcUI.Controllers
{


    /*RoomController, müşterilerin otel odalarını görüntüleyebileceği,
     * yorum bırakabileceği ve detaylara ulaşabileceği web sitesi tarafındaki odaları yöneten controller'dır.

Index action’ında tüm odalar çekilir, aynı oda tipinden sadece bir tanesi gösterilerek liste sadeleştirilmiştir.

Oda fiyatları, sabit veri yerine Web API ile dinamik olarak RoomTypePriceApiClient aracılığıyla alınmaktadır.

Ayrıca odaların yorum puanları ve onaylı yorum sayıları ReviewManager üzerinden hesaplanıp her oda kartına eklenmiştir.

Details action’ında seçilen oda tipine ait ilk müsait oda ve ilişkili görseller Include() ile alınır.(Include() kullanılır çünkü Room ile
    ilişkili görselleri (RoomImages) tek sorguda veritabanından çekerek performansı artırır ve null riskini önler.)

Yine API’den fiyat alınır, yorumlar çekilir ve yalnızca onaylı olanlar ReviewDisplayModel formatında View’a taşınır.

Login olan kullanıcılar için yorum ekleme imkanı sunulmuştur.

AddReview post action’ı ile kullanıcıdan gelen yorumlar DTO olarak alınır ve onay bekleyecek şekilde işaretlenerek sisteme kaydedilir.*/


    [Route("Rooms")]
    public class RoomController : Controller
    {
        private readonly IRoomManager _roomManager;
        private readonly IMapper _mapper;
        private readonly RoomTypePriceApiClient _roomTypePriceApiClient;
        private readonly IReviewManager _reviewManager;
        private readonly IReservationManager _reservationManager;

        public RoomController(IRoomManager roomManager, IMapper mapper, RoomTypePriceApiClient roomTypePriceApiClient, IReviewManager reviewManager, IReservationManager reservationManager)
        {
            _roomManager = roomManager;
            _mapper = mapper;
            _roomTypePriceApiClient = roomTypePriceApiClient;
            _reviewManager = reviewManager;
            _reservationManager = reservationManager;
        }

        /// <summary>
        /// Web sitesinde her oda tipinden birer örneği listeler. 
        /// Fiyatlar API'den çekilir, yorum ortalaması ve yorum sayısı eklenir.
        /// </summary>
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<RoomDto> rooms = await _roomManager.GetAllWithImagesAsync();

            // Aynı oda tipinden sadece bir örnek alınır
            List<RoomDto> distinctRooms = rooms
                .GroupBy(r => r.RoomType)
                .Select(group => group.First())
                .ToList();

            // DTO → ViewModel
            List<RoomResponseModel> modelList = _mapper.Map<List<RoomResponseModel>>(distinctRooms);

            foreach (RoomResponseModel room in modelList)
            {
                // Fiyatı API'den al
                decimal? apiPrice = await _roomTypePriceApiClient.GetPriceByRoomTypeAsync(room.RoomType);
                if (apiPrice.HasValue)
                    room.PricePerNight = apiPrice.Value;

                // Ortalama puan ve onaylı yorum sayısı
                room.AverageRating = await _reviewManager.GetAverageRatingByRoomTypeAsync(room.RoomType);
                List<ReviewDto> reviews = await _reviewManager.GetReviewsByRoomTypeAsync(room.RoomType);
                room.ReviewCount = reviews.Count(r => r.IsApproved);
            }

            return View(modelList);
        }

        /// <summary>
        /// Seçilen oda tipinin detaylarını gösterir. Görseller, yorumlar ve ortalama puan yüklenir.
        /// </summary>
        [HttpGet("Room/Details")]
        public async Task<IActionResult> Details(RoomType roomType)
        {
            // 1️⃣ Veritabanından bu oda tipine ait ilk odayı ve görsellerini alıyoruz
            Room? room = await _roomManager.GetFirstOrDefaultAsync(
                predicate: x => x.RoomType == roomType,
                include: x => x.Include(r => r.RoomImages) // ilişkili RoomImages tablosunu da dahil et
            );

            // 2️⃣ Oda bulunamadıysa 404 dön
            if (room == null)
                return NotFound();

            // 3️⃣ Room → RoomDto → RoomResponseModel (ViewModel) dönüşümü
            RoomDto roomDto = _mapper.Map<RoomDto>(room);
            RoomResponseModel model = _mapper.Map<RoomResponseModel>(roomDto);

            // 4️⃣ Fiyat bilgisi RoomType üzerinden API'den çekilir
            decimal? apiPrice = await _roomTypePriceApiClient.GetPriceByRoomTypeAsync(model.RoomType);
            if (apiPrice.HasValue)
                model.PricePerNight = apiPrice.Value;

            // 5️⃣ Ortalama puan hesaplanır (veritabanından)
            model.AverageRating = await _reviewManager.GetAverageRatingByRoomTypeAsync(model.RoomType);

            // 6️⃣ Oda tipine ait yorumlar alınır
            List<ReviewDto> reviews = await _reviewManager.GetReviewsByRoomTypeAsync(model.RoomType);
            model.ReviewCount = reviews.Count;

            // 7️⃣ Yorumlardan sadece onaylı olanlar filtrelenir ve ViewModel’e dönüştürülür
            model.reviewDisplays = reviews
                .Where(r => r.IsApproved)
                .OrderByDescending(r => r.CommentDate)
                .Select(r => new ReviewDisplayModel
                {
                    CustomerName = r.IsAnonymous ? "Anonim Kullanıcı" : "Kullanıcı",
                    Comment = r.Comment,
                    Rating = r.Rating,
                    CreatedDate = r.CommentDate
                }).ToList();

            // 8️⃣ Kullanıcı giriş yaptıysa yorum yapabilsin diye flag gönderiyoruz
            model.CanComment = User.Identity != null && User.Identity.IsAuthenticated;

            // 9️⃣ View’a modeli gönderiyoruz
            return View("Details", model);
        }


        /// <summary>
        /// Kullanıcının yaptığı yorumu alır ve veritabanına kaydeder. 
        /// Yorum onaylı değilse gösterilmez.
        /// </summary>
        [HttpPost("Room/AddReview")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(ReviewDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Yorum gönderilirken hata oluştu. Lütfen formu kontrol edin.";
                return RedirectToAction("Details", new { roomType = model.RoomType });
            }

            // Yorum durumu varsayılan olarak onaysızdır
            model.CommentDate = DateTime.Now;
            model.IsApproved = false;

            bool result = await _reviewManager.AddReviewAsync(model);

            TempData[result ? "Success" : "Error"] = result
                ? "Yorumunuz alındı. Onaylandıktan sonra yayınlanacaktır."
                : "Yorum gönderilemedi.";

            return RedirectToAction("Details", new { roomType = model.RoomType });
        }
    }
}
