using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Core;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.Models.PureVm.RequestModel.Reservation;
using Project.MvcUI.Services;
using System.Security.Claims;

namespace Project.MvcUI.Controllers
{
    [Authorize]
    [Route("Reservation")]
    public class ReservationController : Controller
    {
        private readonly IRoomManager _roomManager;
        private readonly IReservationManager _reservationManager;
        private readonly IEarlyReservationDiscountManager _earlyReservationDiscountManager;
        private readonly ICustomerManager _customerManager;
        private readonly IMapper _mapper;
        private readonly IRoomTypePriceManager _roomTypePriceManager;
        private readonly RoomTypePriceApiClient _apiClient;

        public ReservationController(
            IRoomManager roomManager,
            IMapper mapper,
            IReservationManager reservationManager,
            IEarlyReservationDiscountManager earlyReservationDiscountManager,
            ICustomerManager customerManager,
            IRoomTypePriceManager roomTypePriceManager, RoomTypePriceApiClient apiClient)
        {
            _roomManager = roomManager;
            _mapper = mapper;
            _reservationManager = reservationManager;
            _earlyReservationDiscountManager = earlyReservationDiscountManager;
            _customerManager = customerManager;
            _roomTypePriceManager = roomTypePriceManager;
            _apiClient = apiClient;
        }

        // 🔹 Kullanıcı oda seçmeden gelirse yönlendir
        [HttpGet("Create")]
        public IActionResult Create()
        {
            TempData["Message"] = "Lütfen önce bir oda seçerek rezervasyona başlayınız.";
            return RedirectToAction("Index", "Room");
        }

        [HttpGet("CreateByRoomType")]
        public async Task<IActionResult> CreateByRoomType(RoomType roomType)
        {
            try
            {
                // 1. Uygun oda bulunamazsa View'a yönlendir
                Room roomEntity = await _roomManager.GetFirstOrDefaultAsync(
                    r => r.RoomType == roomType && r.RoomStatus == RoomStatus.Available,
                    include => include.Include(r => r.RoomImages)
                );

                if (roomEntity == null)
                {
                    ViewBag.IsAvailable = false;
                    TempData["Message"] = "Bu tipte boş oda bulunamadı.";
                    return View("Create", new ReservationRequestModel());
                }

                ViewBag.IsAvailable = true;
                RoomDto room = _mapper.Map<RoomDto>(roomEntity);

                DateTime checkIn = DateTime.Today.AddDays(1);
                DateTime checkOut = DateTime.Today.AddDays(3);
                int duration = (checkOut - checkIn).Days;

                // 2. Fiyatı API'den çekiyoruz, try-catch ile saralım
                decimal? basePrice;
                try
                {
                    basePrice = await _apiClient.GetPriceByRoomTypeAsync(roomType);
                }
                catch
                {
                    TempData["Message"] = "Fiyat bilgisi alınamadı. Lütfen tekrar deneyin.";
                    return View("Create", new ReservationRequestModel());
                }

                if (basePrice == null)
                {
                    TempData["Message"] = "Fiyat bilgisi bulunamadı.";
                    return View("Create", new ReservationRequestModel());
                }

                // 3. Paket türü varsayılan: Tam Pansiyon
                ReservationPackage package = ReservationPackage.Fullboard;

                // 4. Kullanıcı login olmuşsa, indirim hesaplanabilir
                int customerId = 0;
                if (User.Identity.IsAuthenticated)
                {
                    string? userIdStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    if (!string.IsNullOrEmpty(userIdStr) && int.TryParse(userIdStr, out int userId))
                    {
                        var customer = await _customerManager.GetByUserIdAsync(userId);
                        if (customer != null)
                            customerId = customer.Id;
                    }
                }

                // 5. İndirim hesaplama
                decimal discountRate = await _earlyReservationDiscountManager
                    .CalculateDiscountAsync(customerId, DateTime.Today, checkIn, basePrice.Value, package);

                decimal totalPrice = basePrice.Value * duration * (1 - discountRate / 100);

                // 6. Modeli View'a gönder
                ReservationRequestModel model = new ReservationRequestModel
                {
                    RoomId = room.Id,
                    RoomType = room.RoomType.ToString(),
                    PricePerNight = basePrice.Value,
                    Package = package,
                    CheckIn = checkIn,
                    CheckOut = checkOut,
                    Duration = duration,
                    DiscountRate = discountRate,
                    GuestCount = 1,
                    TotalPrice = totalPrice
                };

                return View("Create", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("🚨 Rezervasyon oluşturulurken hata: " + ex.Message);
                TempData["Message"] = "Beklenmeyen bir hata oluştu.";
                return View("Create", new ReservationRequestModel());
            }
        }


        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ReservationRequestModel model)
        {

            Console.WriteLine("📥 [Reservation] Form gönderildi. CheckIn: " + model.CheckIn + ", CheckOut: " + model.CheckOut);

            if (model.CheckOut <= model.CheckIn)
            {
                ModelState.AddModelError("", "Çıkış tarihi, giriş tarihinden sonra olmalıdır.");
                return View(model);
            }

            if (!ModelState.IsValid)
                return View(model);

            // Kullanıcı ID'si güvenli alınır
            if (!User.Identity.IsAuthenticated)
            {
                TempData["LoginWarning"] = "Devam etmek için giriş yapmanız gerekmektedir!";
                return RedirectToAction("Login", "Account");
            }

            string? userIdStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                TempData["LoginWarning"] = "Kullanıcı bilgisi alınamadı.";
                return RedirectToAction("Login", "Account");
            }
            Console.WriteLine("👤 Kullanıcı ID: " + userId);

            // Ad Soyad güvenli ayrıştırılır
            string[] nameParts = model.FullName?.Split(" ", StringSplitOptions.RemoveEmptyEntries) ?? [];
            string firstName = nameParts.FirstOrDefault() ?? "Ad";
            string lastName = nameParts.Skip(1).FirstOrDefault() ?? "Soyad";


            // Müşteri oluşturuluyor
            var customerDto = await _customerManager.GetOrCreateCustomerAsync(
                userId,
                model.IdentityNumber ?? "00000000000",
                firstName,
                lastName,
                1990
            );
            Console.WriteLine("✅ Müşteri oluşturuldu: " + customerDto.FirstName + " " + customerDto.LastName + ", CustomerId: " + customerDto.Id);

            if (customerDto == null)
            {
                ModelState.AddModelError("", "Müşteri oluşturulamadı.");
                return View(model);
            }

            // RoomType güvenli şekilde parse edilir
            if (!Enum.TryParse<RoomType>(model.RoomType, out RoomType roomTypeEnum))
            {
                ModelState.AddModelError("", "Oda tipi geçersiz.");
                return View(model);
            }
            Console.WriteLine("🏨 RoomType parse edildi: " + roomTypeEnum.ToString());
            // API'den fiyat çekilir (güvenli şekilde)
            decimal? basePrice;
            try
            {
                basePrice = await _apiClient.GetPriceByRoomTypeAsync(roomTypeEnum);
            }
            catch
            {
                ModelState.AddModelError("", "Fiyat bilgisi alınırken hata oluştu.");
                return View(model);
            }

            if (basePrice == null)
            {
                ModelState.AddModelError("", "Fiyat bilgisi bulunamadı.");
                return View(model);
            }

            model.PricePerNight = basePrice.Value;
            Console.WriteLine("💰 Fiyat bilgisi alındı: " + basePrice + " ₺");

            // Süre ve indirim hesaplanır
            int duration = (model.CheckOut - model.CheckIn).Days;
            model.Duration = duration;

            model.DiscountRate = await _earlyReservationDiscountManager
                .CalculateDiscountAsync(customerDto.Id, DateTime.Today, model.CheckIn, basePrice.Value, model.Package);

            decimal discountMultiplier = 1 - (model.DiscountRate / 100);
            model.TotalPrice = basePrice.Value * duration * discountMultiplier;

            if (model.TotalPrice < 0)
                model.TotalPrice = 0;

            // Rezervasyon kaydı yapılır
            int reservationId = await _reservationManager.CreateAndReturnIdAsync(
     customerDto.Id, userId, model.RoomId, model.CheckIn, duration, model.Package, model.TotalPrice);
            Console.WriteLine("🔢 Süre: " + duration + " gün, İndirim: %" + model.DiscountRate);
            Console.WriteLine("💸 Toplam Fiyat (indirimli): " + model.TotalPrice);
            Console.WriteLine("📦 Rezervasyon oluşturuluyor...");
            if (reservationId <= 0)
            {
                ModelState.AddModelError("", "Rezervasyon oluşturulamadı.");
                return View(model);
            }
            Console.WriteLine("🎫 Rezervasyon kaydedildi. ID: " + reservationId);

            return RedirectToAction("Pay", "Payment", new { reservationId = reservationId });
        }
    }
 }
