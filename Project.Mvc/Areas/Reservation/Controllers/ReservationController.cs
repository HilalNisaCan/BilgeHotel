using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.BLL.Services.abstracts;
using Project.Entities.Enums;
using Project.MvcUI.Areas.Reservation.Models.PageVm;
using Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Reservation;
using Project.MvcUI.Areas.Reservation.Models.PureVm.ResponseModel.Reservation;
using Project.MvcUI.Models.PureVm.ResponseModel.Room;

namespace Project.MvcUI.Areas.Reservation.Controllers
{



    /*“Reservation Area’daki bu controller, resepsiyonistlerin otelde fiziksel olarak gelen müşterileri sisteme kaydetmesini sağlar.
     * Admin alanından bağımsız çalışır ve kullanıcı doğrulaması, kimlik kontrolü, rezervasyon oluşturma ve anında ödeme alma işlemlerini aynı işlem akışında tamamlar.
     * DTO ve ViewModel katmanları ayrıştırılmış, AutoMapper entegrasyonu sağlanmış ve gerekli iş kuralları Business Layer’da çözülmüştür.”*/



    [Area("Reservation")]
    public class ReservationController : Controller
    {
        private readonly IRoomManager _roomManager;
        private readonly IRoomTypePriceManager _roomTypePriceManager;
        private readonly IPaymentManager _paymentManager;
        private readonly ICustomerManager _customerManager;
        private readonly IMapper _mapper;
        private readonly IReservationManager _reservationManager;
     

        public ReservationController(IRoomManager roomManager, IRoomTypePriceManager roomTypePriceManager, IMapper mapper,ICustomerManager customerManager,IReservationManager reservationManager,IPaymentManager paymentManager)
        {
            _roomManager = roomManager;
            _roomTypePriceManager = roomTypePriceManager;
            _mapper = mapper;
            _customerManager = customerManager;
            _reservationManager = reservationManager;
            _paymentManager = paymentManager;
        
        }




        /// <summary>
        /// Oda listesi + rezervasyon formunu içeren ana sayfa (GET)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RoomDto> roomDtoList = await _roomManager.GetAllWithImagesAsync();
            List<RoomTypePriceDto> priceDtoList = await _roomTypePriceManager.GetAllAsync();

            List<RoomResponseModel> roomList = _mapper.Map<List<RoomResponseModel>>(roomDtoList,
                opt => opt.Items["RoomTypePrices"] = priceDtoList);

            ReservationPageVm vm = new ReservationPageVm
            {
                ReservationForm = new CreateReservationRequestModel(),
                AvailableRooms = roomList
            };

            return View(vm);
        }

        /// <summary>
        /// Yeni rezervasyon formu (belirli bir oda seçilmiş olarak)
        /// </summary>
        [HttpGet("Create")]
        public async Task<IActionResult> CreateAsync(int roomId)
        {
            List<RoomDto> roomDtos = await _roomManager.GetAllWithPricesAsync();
            List<RoomResponseModel> roomList = _mapper.Map<List<RoomResponseModel>>(roomDtos);

            CreateReservationRequestModel model = new CreateReservationRequestModel
            {
                RoomId = roomId,
                RoomList = roomList
            };

            return View(model);
        }

        /// <summary>
        /// Resepsiyon üzerinden rezervasyon oluşturur (anonim müşteriyle birlikte).
        /// Kimlik doğrulaması yapılır, müşteri eklenir, rezervasyon oluşturulur ve ödeme kaydı eklenir.
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateReservationRequestModel model)
        {
            // 1️⃣ Form doğrulama başarısızsa sayfa tekrar gösterilir
            if (!ModelState.IsValid)
            {
                Console.WriteLine("⛔ ModelState geçersiz.");
                await PopulateRoomListAsync(model);
                return View(model);
            }

            //// 2️⃣ T.C. Kimlik doğrulama yapılır
            KimlikBilgisiDto kimlik = _mapper.Map<KimlikBilgisiDto>(model);
            bool isIdentityVerified = await _customerManager.VerifyCustomerIdentityAsync(kimlik);

            if (!isIdentityVerified)
            {
               Console.WriteLine("❌ TC doğrulama başarısız.");
                ModelState.AddModelError("IdentityNumber", "T.C. Kimlik numarası doğrulanamadı.");
                await PopulateRoomListAsync(model);
                return View(model);
            }

            try
            {
                // 3️⃣ Müşteri DTO'su oluşturulur
                CustomerDto customerDto = _mapper.Map<CustomerDto>(model);
                customerDto.UserId = null; // Sistem kullanıcısı değil → anonim misafir
                customerDto.IsIdentityVerified = true;

                // 4️⃣ Müşteri veritabanına kaydedilir
                int customerId = await _customerManager.AddAsync(customerDto);
                Console.WriteLine($"✅ Müşteri eklendi: {customerId}");

                // 5️⃣ Rezervasyon veritabanına EF ile yazılır ve ID alınır
                int reservationId = await _reservationManager.CreateAndReturnIdAsync(
                    customerId,
                    model.RoomId,
                    model.CheckIn,
                    model.Duration,
                    model.Package,
                    model.TotalPrice
                );

                Console.WriteLine($"✅ Rezervasyon EF ile eklendi: {reservationId}");

                // 6️⃣ Ödeme DTO'su oluşturulur
                PaymentDto payment = new PaymentDto
                {
                    ReservationId = reservationId,
                    CustomerId = customerId,
                    UserId = null, // Kullanıcı giriş yapmadığı için null
                    TotalAmount = model.TotalPrice,
                    PaidAmount = model.TotalPrice,
                    PaymentStatus = PaymentStatus.Completed,
                    PaymentMethod = PaymentMethod.Cash,
                    Description = $"Otel içi ödeme - Rezervasyon #{reservationId}",
                    InvoiceNumber = $"OTEL-{Guid.NewGuid().ToString("N")[..8]}",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                Console.WriteLine($"✅ Ödeme hazırlandı: {payment.InvoiceNumber}");

                // 7️⃣ Ödeme kaydı veritabanına eklenir
                await _paymentManager.AddAsync(payment);

                // ✅ Başarı mesajı gösterilir ve yönlendirme yapılır
                TempData["Success"] = "Rezervasyon başarıyla oluşturuldu.";
                return RedirectToAction("Index", "Dashboard", new { area = "Reservation" });
            }
            catch (Exception ex)
            {
                // ❌ Herhangi bir hata durumunda kullanıcı bilgilendirilir
                Console.WriteLine("❌ HATA: " + ex.Message);
                Console.WriteLine("📄 StackTrace: " + ex.StackTrace);
                ModelState.AddModelError("", "Bir hata oluştu. Lütfen tekrar deneyin.");
                await PopulateRoomListAsync(model);
                return View(model);
            }
        }

        /// <summary>
        /// RoomList bilgilerini doldurur (View için)
        /// </summary>
        private async Task PopulateRoomListAsync(CreateReservationRequestModel model)
        {
            List<RoomDto> rooms = await _roomManager.GetAllWithPricesAsync();
            model.RoomList = _mapper.Map<List<RoomResponseModel>>(rooms);
        }





        /// <summary>
        /// Seçilen odanın fiyatını JSON olarak döner (API)
        /// </summary>
        [HttpGet("api/room/price/{roomId}")]
        public async Task<IActionResult> GetRoomPriceByRoomId(int roomId)
        {
            RoomDto? room = await _roomManager.GetByIdAsync(roomId);
            if (room == null)
                return NotFound();

            RoomTypePriceDto? priceDto = await _roomTypePriceManager.GetByRoomTypeAsync(room.RoomType);
            if (priceDto == null)
                return NotFound(new { price = 0 });

            return Ok(new { price = priceDto.PricePerNight });
        }
    }
}