using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Room;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Room;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Rooms")]
    public class RoomController : Controller
    {
        private readonly IRoomManager _roomManager;
        private readonly IRoomTypePriceManager _roomTypePriceManager;
        private readonly IMapper _mapper;

        public RoomController(IRoomManager roomManager, IMapper mapper, IRoomTypePriceManager roomTypePriceManager)
        {
            _roomManager = roomManager;
            _mapper = mapper;
            _roomTypePriceManager = roomTypePriceManager;
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

            RoomAdminResponseModel vm = _mapper.Map<RoomAdminResponseModel>(
                roomDto,
                opt => opt.Items["RoomTypePrices"] = priceList // ✔ context’e fiyat listesi veriyoruz
            );

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
