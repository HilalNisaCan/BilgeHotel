using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Room;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Room;
using Microsoft.EntityFrameworkCore;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Rooms")]
    public class RoomController : Controller
    {
        private readonly IRoomManager _roomManager;
        private readonly IRoomTypePriceManager _roomTypePriceManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IRoomCleaningScheduleManager _roomCleaningScheduleManager;
        private readonly IRoomMaintenanceAssignmentManager _roomMaintenanceAssignmentManager;
        private readonly IRoomMaintenanceManager _roomMaintenanceManager;

        private readonly IMapper _mapper;

        public RoomController(IRoomManager roomManager, IMapper mapper, IRoomTypePriceManager roomTypePriceManager, IEmployeeManager employeeManager,
    IRoomCleaningScheduleManager roomCleaningScheduleManager, IRoomMaintenanceAssignmentManager roomMaintenanceAssignmentManager, IRoomMaintenanceManager roomMaintenanceManager)
        {
            _roomManager = roomManager;
            _mapper = mapper;
            _roomTypePriceManager = roomTypePriceManager;
            _employeeManager = employeeManager;
            _roomCleaningScheduleManager = roomCleaningScheduleManager;
            _roomMaintenanceAssignmentManager = roomMaintenanceAssignmentManager;
            _roomMaintenanceManager = roomMaintenanceManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(RoomFilterVm filter)
        {
            // Tüm odalarla başla
            List<RoomDto> roomDtoList = await _roomManager.GetAllWithImagesAsync();
            List<RoomTypePriceDto> priceDtoList = await _roomTypePriceManager.GetAllAsync();

            // Filtreleme işlemleri
            if (!string.IsNullOrEmpty(filter.RoomType))
            {
                roomDtoList = roomDtoList.Where(r => r.RoomType.ToString() == filter.RoomType).ToList();
            }

            if (!string.IsNullOrEmpty(filter.RoomStatus))
            {
                roomDtoList = roomDtoList.Where(r => r.Status.ToString() == filter.RoomStatus).ToList();
            }

            if (filter.MinPrice.HasValue)
            {
                roomDtoList = roomDtoList.Where(r => r.PricePerNight >= filter.MinPrice).ToList();
            }

            if (filter.MaxPrice.HasValue)
            {
                roomDtoList = roomDtoList.Where(r => r.PricePerNight <= filter.MaxPrice).ToList();
            }

            // Veriyi modele mapa
            List<RoomAdminResponseModel> vmList = _mapper.Map<List<RoomAdminResponseModel>>(roomDtoList,
                opt => opt.Items["RoomTypePrices"] = priceDtoList);

            return View(vmList);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            RoomDto roomDto = await _roomManager.GetByIdWithImagesAsync(id);
            if (roomDto == null)
                return NotFound();

            List<RoomTypePriceDto> priceList = await _roomTypePriceManager.GetAllAsync();

            // 🔵 Temizlik bilgisi sadece "Cleaning" durumundaysa getiriyoruz
            RoomCleaningScheduleDto? cleaningInfo = null;
            if (roomDto.Status == RoomStatus.Cleaning)
            {
                cleaningInfo = await _roomCleaningScheduleManager.GetLatestByRoomIdAsync(id);
            }

            // 🟡 Bakım bilgisi her zaman kontrol edilsin
            RoomMaintenanceAssignmentDto? maintenanceInfo = await _roomMaintenanceAssignmentManager.GetLatestByRoomIdAsync(id);

            RoomAdminResponseModel vm = _mapper.Map<RoomAdminResponseModel>(
                roomDto,
                opt => opt.Items["RoomTypePrices"] = priceList
            );

            vm.CleaningInfo = cleaningInfo;
            vm.MaintenanceInfo = maintenanceInfo;

            return View(vm);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            RoomDto roomDto = await _roomManager.GetByIdAsync(id);
            if (roomDto == null)
                return NotFound();

            RoomUpdateVm vm = _mapper.Map<RoomUpdateVm>(roomDto);
            return View(vm);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, RoomUpdateVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // Mevcut DTO'yu EF ile takipte olan haliyle çekiyoruz
            RoomDto existingRoom = await _roomManager.GetByIdAsync(id);
            if (existingRoom == null)
                return NotFound();

            // Güncellenebilir alanları manuel atıyoruz
            existingRoom.RoomNumber = vm.RoomNumber;
            existingRoom.FloorNumber = vm.FloorNumber;
            existingRoom.Status = vm.Status;
            existingRoom.Description = vm.Description;
            existingRoom.HasMinibar = vm.HasMinibar;
            existingRoom.HasHairDryer = vm.HasHairDryer;
            existingRoom.HasTV = vm.HasTV;
            existingRoom.HasBalcony = vm.HasBalcony;
            existingRoom.HasAirConditioning = vm.HasAirConditioning;
            existingRoom.HasWirelessInternet = vm.HasWiFi;

            await _roomManager.UpdateAsync(existingRoom);

            return RedirectToAction("Index");
        }

        [HttpGet("AssignCleaning/{roomId}")]
        public async Task<IActionResult> AssignCleaning(int roomId)
        {
            // Odayı kontrol et
            RoomDto room = await _roomManager.GetByIdAsync(roomId);
            if (room == null || room.Status != RoomStatus.Cleaning)
                return RedirectToAction("Index");

            // Temizlik personellerini getir
            List<EmployeeDto> cleaners = await _employeeManager.GetByPositionAsync(EmployeePosition.Cleaner);

            ViewBag.Cleaners = new SelectList(cleaners, "Id", "FullName");

            // Boş model oluşturup sadece RoomId set ediliyor
            RoomCleaningScheduleCreateRequestModel model = new RoomCleaningScheduleCreateRequestModel
            {
                RoomId = roomId
            };

            return View("AssignCleaning", model);
        }
        [HttpPost("AssignCleaning")]
        public async Task<IActionResult> AssignCleaning(RoomCleaningScheduleCreateRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Form geçerli değil.";
                return RedirectToAction("AssignCleaning", new { roomId = model.RoomId });
            }

            RoomCleaningSchedule entity = new RoomCleaningSchedule
            {
                RoomId = model.RoomId,
                AssignedEmployeeId = model.AssignedEmployeeId,
                ScheduledDate = model.ScheduledDate,
                Description = model.Description,
                CleaningStatus = CleaningStatus.Scheduled,
                IsCompleted = false,
                Status = DataStatus.Inserted,
            };

            await _roomCleaningScheduleManager.CreateAndConfirmAsync(entity);

            TempData["Message"] = "Temizlik görevlisi başarıyla atandı.";
            return RedirectToAction("Details", new { id = model.RoomId });
        }



        [HttpGet("AssignMaintenance/{roomId}")]
        public async Task<IActionResult> AssignMaintenance(int roomId)
        {
            RoomDto room = await _roomManager.GetByIdAsync(roomId);
            if (room == null || room.Status != RoomStatus.Maintenance)
                return RedirectToAction("Index");

            // Elektrikçi ve IT personellerini getir
            List<EmployeeDto> maintainers = await _employeeManager.GetByPositionsAsync(
                new[] { EmployeePosition.Electrician, EmployeePosition.ITSpecialist });

            ViewBag.Maintainers = new SelectList(maintainers, "Id", "FullName");

            RoomMaintenanceAssignmentCreateRequestModel model = new RoomMaintenanceAssignmentCreateRequestModel
            {
                RoomId = roomId,
                AssignedDate = DateTime.Now
            };

            return View("AssignMaintenance", model);
        }

        [HttpPost("AssignMaintenance/{roomId}")]
        public async Task<IActionResult> AssignMaintenance(RoomMaintenanceAssignmentCreateRequestModel model)
        {
            Console.WriteLine($"🧪 DEBUG | Seçilen EmployeeId: {model.EmployeeId}");

            // 1. Employee gerçekten var mı?
            var employee = await _employeeManager.GetByIdAsync(model.EmployeeId);
            if (employee == null)
            {
                TempData["Message"] = "Geçersiz personel seçimi!";
                return RedirectToAction("AssignMaintenance", new { roomId = model.RoomId });
            }

            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Form geçerli değil.";
                return RedirectToAction("AssignMaintenance", new { roomId = model.RoomId });
            }

            // 2. Bakım kaydı kontrolü ve oluşturulması
            int maintenanceId = await _roomMaintenanceManager.GetOrCreateTodayMaintenanceAsync(
                model.RoomId,
                model.MaintenanceType);

            // 3. Atama kaydı oluşturuluyor
            RoomMaintenanceAssignment entity = new RoomMaintenanceAssignment
            {
                RoomId = model.RoomId,
                RoomMaintenanceId = maintenanceId,
                EmployeeId = model.EmployeeId,
                AssignedDate = model.AssignedDate,
                MaintenanceStatus = MaintenanceStatus.Scheduled,
                Description = model.Description,
                Status = DataStatus.Inserted
            };

            // 4. Kaydet
            await _roomMaintenanceAssignmentManager.CreateWithEntityAsync(entity);

            TempData["Message"] = "Bakım görevlisi başarıyla atandı.";
            return RedirectToAction("Details", new { id = model.RoomId });
        }


        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            RoomDto dto = await _roomManager.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            await _roomManager.RemoveAsync(dto);
            return RedirectToAction("Index");
        }

    }

}
