using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging.Core;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Reservation;
using Project.MvcUI.Models.PageVm.Reservation;
using Project.MvcUI.Models.PureVm.RequestModel.Reservation;
using Project.MvcUI.Services;
using System.Security.Claims;

namespace Project.MvcUI.Controllers
{

    /// 🛎️ ReservationController
/*“ReservationController, müşterilerin web sitesi üzerinden oda rezervasyonu yapmasını sağlar.
Oda tipi seçimine göre API’den fiyat verisi alınır, erken rezervasyon indirimi hesaplanır ve kullanıcı doğrulanarak işlem tamamlanır.
Form verileri doğrudan veritabanına yazılmaz, önce TempModel ile ödeme ekranına yönlendirilir.
Böylece güvenli ve kullanıcı dostu bir rezervasyon süreci elde edilir.”*/ 

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
    /// <summary>
    /// Kullanıcı oda seçmeden rezervasyon sayfasına gelirse yönlendirme yapılır.
    /// </summary>
    [HttpGet("Create")]
    public IActionResult Create()
    {
        TempData["Message"] = "Lütfen önce bir oda seçerek rezervasyona başlayınız.";
        return RedirectToAction("Index", "Room");
    }

    /// <summary>
    /// Oda tipi seçilerek rezervasyon formu açılır. Uygun oda, fiyat ve indirim bilgisi alınır.
    /// </summary>
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

            //"Tek bir mapleme ile hepsini çözmek mümkün olmadığı için,
            //business logic'in çıktıları controller'da toplandıktan sonra ViewModel’e açık tiplerle atandı
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

    /// <summary>
    /// Kullanıcının formdan gönderdiği rezervasyon bilgileri alınır.
    /// TempData aracılığıyla ödeme ekranına yönlendirilir.
    /// </summary>
    [Authorize]
    [HttpPost("Create")]
    public async Task<IActionResult> Create(ReservationRequestModel model)
    {
        // 📌 Geçerli tarih aralığı kontrolü
        if (model.CheckOut <= model.CheckIn)
        {
            ModelState.AddModelError("", "Çıkış tarihi, giriş tarihinden sonra olmalıdır.");
            return View(model);
        }

        if (!ModelState.IsValid)
            return View(model);

        // 👤 Giriş yapan kullanıcıdan ID alınır
        string? userIdStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
        {
            TempData["LoginWarning"] = "Kullanıcı bilgisi alınamadı.";
            return RedirectToAction("Login", "Account");
        }

        // 🧍 Ad soyad ayrıştırılır
        string[] nameParts = model.FullName?.Split(" ", StringSplitOptions.RemoveEmptyEntries) ?? [];
        string firstName = nameParts.FirstOrDefault() ?? "Ad";
        string lastName = nameParts.Skip(1).FirstOrDefault() ?? "Soyad";

        // 👥 Müşteri oluşturma veya getirme işlemi
        CustomerDto customerDto = await _customerManager.GetOrCreateCustomerAsync(
            userId,
            model.IdentityNumber ?? "00000000000",
            firstName,
            lastName,
            1990 // varsayılan yıl
        );

        // ❗ RoomType string → enum dönüşümü
        if (!Enum.TryParse<RoomType>(model.RoomType, out RoomType roomTypeEnum))
        {
            ModelState.AddModelError("", "Oda tipi geçersiz.");
            return View(model);
        }

        // 💰 Güncel fiyat API'den çekilir
        decimal? basePrice;
        try
        {
            basePrice = await _apiClient.GetPriceByRoomTypeAsync(roomTypeEnum);
            Console.WriteLine($"🎯 Gelen baz fiyat (API): {basePrice}");
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

        // 🧮 Süre ve indirim hesaplanır
        int duration = (model.CheckOut - model.CheckIn).Days;

        decimal discountRate = await _earlyReservationDiscountManager
            .CalculateDiscountAsync(customerDto.Id, DateTime.Today, model.CheckIn, basePrice.Value, model.Package);

        decimal totalPrice = basePrice.Value * duration * (1 - discountRate / 100);

            /// ✅ AutoMapper ile formdan gelen veriler ReservationTempModel'e aktarılır.
            // 🎯 Ardından sistem içinde hesaplanan alanlar (UserId, fiyat, süre, indirim) elle tamamlanır.
            // ✅ AutoMapper ile TempModel oluştur
            //"Rezervasyon verisi, ödeme tamamlanmadan veritabanına yazılmamalı.
            //Bu yüzden form verilerini ReservationTempModel olarak AutoMapper ile dönüştürdüm,
            //sistem içindeki hesaplamaları ekledim ve ödeme ekranına TempData ile taşıyarak işlemi finalize ettim."
            ReservationTempModel tempModel = _mapper.Map<ReservationTempModel>(model);
            tempModel.UserId = userId;
            tempModel.CustomerId = customerDto.Id;
            tempModel.RoomType = roomTypeEnum;
            tempModel.PricePerNight = model.PricePerNight;
            tempModel.Duration = model.Duration;
            tempModel.TotalPrice = model.TotalPrice;
            tempModel.DiscountRate = model.DiscountRate;
            tempModel.NumberOfGuests = model.GuestCount;
            tempModel.Package = model.Package;
            tempModel.CheckIn = model.CheckIn;
            tempModel.CheckOut = model.CheckOut;


            // 💾 TempData ile ödeme ekranına aktar
            TempData["TempReservation"] = JsonConvert.SerializeObject(tempModel);
        // ✅ Ödeme ekranına yönlendirilir
        ReservationPaymentPageVm paymentVm = new ReservationPaymentPageVm
        {
            TotalPrice = totalPrice
        };

        return RedirectToAction("Pay", "Payment");

    }
}
}
