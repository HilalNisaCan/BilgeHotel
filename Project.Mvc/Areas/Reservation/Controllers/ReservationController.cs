using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.Entities.Enums;
using Project.MvcUI.Areas.Reservation.Models.PageVm;
using Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Reservation;
using Project.MvcUI.Areas.Reservation.Models.PureVm.ResponseModel.Reservation;
using Project.MvcUI.Models.PureVm.ResponseModel.Room;

namespace Project.MvcUI.Areas.Reservation.Controllers
{
    [Area("Reservation")]
    public class ReservationController : Controller
    {
        private readonly IRoomManager _roomManager;
        private readonly IRoomTypePriceManager _roomTypePriceManager;
        private readonly ICustomerManager _customerManager;
        private readonly IMapper _mapper;
        private readonly IReservationManager _reservationManager;

        public ReservationController(IRoomManager roomManager, IRoomTypePriceManager roomTypePriceManager, IMapper mapper,ICustomerManager customerManager,IReservationManager reservationManager)
        {
            _roomManager = roomManager;
            _roomTypePriceManager = roomTypePriceManager;
            _mapper = mapper;
            _customerManager = customerManager;
            _reservationManager = reservationManager;
        }

   
      

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Odaları ve fiyat bilgilerini çekiyoruz
            List<RoomDto> roomDtoList = await _roomManager.GetAllWithImagesAsync();
            List<RoomTypePriceDto> priceDtoList = await _roomTypePriceManager.GetAllAsync();

            // AutoMapper ile RoomResponseModel'e çeviriyoruz
            List<RoomResponseModel> roomList = _mapper.Map<List<RoomResponseModel>>(roomDtoList,
                opt => opt.Items["RoomTypePrices"] = priceDtoList);

            // ViewModel’i hazırlıyoruz
            ReservationPageVm vm = new ReservationPageVm
            {
                ReservationForm = new CreateReservationRequestModel(),
                AvailableRooms = roomList
            };

            return View(vm);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> CreateAsync(int roomId)
        {
            List<RoomDto> roomDtos = await _roomManager.GetAllWithPricesAsync();

            CreateReservationRequestModel model = new CreateReservationRequestModel
            {
                RoomId = roomId,
                RoomList = _mapper.Map<List<RoomResponseModel>>(roomDtos)
            };

            return View(model);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateReservationRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                LogModelErrors(ModelState);

                await PopulateRoomListAsync(model);
                return View(model);
            }

            // T.C. Kimlik doğrulaması
            KimlikBilgisiDto kimlik = _mapper.Map<KimlikBilgisiDto>(model);
            bool isIdentityVerified = await _customerManager.VerifyCustomerIdentityAsync(kimlik);

            if (!isIdentityVerified)
            {
                ModelState.AddModelError("IdentityNumber", "T.C. Kimlik numarası doğrulanamadı.");
                await PopulateRoomListAsync(model);
                return View(model);
            }

            try
            {
                // Müşteri oluştur
                CustomerDto customerDto = _mapper.Map<CustomerDto>(model);
                customerDto.UserId = null; // ❗ User kaydı yoksa FK hatasını engellemek için null veriyoruz

                int customerId = await _customerManager.AddAsync(customerDto);

                // Rezervasyon oluştur
                ReservationDto reservation = new ReservationDto
                {
                    CustomerId = customerId,
                    RoomId = model.RoomId,
                    StartDate = model.CheckIn,
                    EndDate = model.CheckOut,
                    NumberOfGuests = model.GuestCount,
                    Package = model.Package,
                    TotalPrice = model.TotalPrice,
                    DiscountRate = (decimal)model.DiscountRate,
                    ReservationStatus = ReservationStatus.Confirmed,
                    CreatedDate = DateTime.Now
                };

                await _reservationManager.AddAsync(reservation);

                TempData["Success"] = "Rezervasyon başarıyla oluşturuldu.";
                return RedirectToAction("Index", "Dashboard", new { area = "Reservation" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 HATA: {ex.Message}\n{ex.StackTrace}");
                ModelState.AddModelError("", "Bir hata oluştu. Lütfen tekrar deneyin.");
                await PopulateRoomListAsync(model);
                return View(model);
            }



        }
        private async Task PopulateRoomListAsync(CreateReservationRequestModel model)
        {
            List<RoomDto> rooms = await _roomManager.GetAllWithPricesAsync();
            model.RoomList = _mapper.Map<List<RoomResponseModel>>(rooms);
        }

        private void LogModelErrors(ModelStateDictionary modelState)
        {
            Console.WriteLine("🛑 ModelState geçersiz. Hatalar:");

            foreach (var entry in modelState)
            {
                var key = entry.Key;
                var attemptedValue = entry.Value?.AttemptedValue ?? "Boş";

                Console.WriteLine($"📌 Alan: {key} | Değer: {attemptedValue}");

                foreach (var error in entry.Value.Errors)
                {
                    Console.WriteLine($"   ⚠️ {error.ErrorMessage}");
                }
            }
        }



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