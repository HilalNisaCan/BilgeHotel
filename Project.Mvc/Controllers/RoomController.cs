using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.Entities.Models;
using Project.MvcUI.Models.PureVm.ResponseModel.Review;
using Project.MvcUI.Models.PureVm.ResponseModel.Room;
using Project.MvcUI.Services;


namespace Project.MvcUI.Controllers
{
    [Route("Rooms")]
    public class RoomController : Controller
    {
        private readonly IRoomManager _roomManager;
        private readonly IMapper _mapper;
        private readonly RoomTypePriceApiClient _roomTypePriceApiClient;

        public RoomController(IRoomManager roomManager, IMapper mapper, RoomTypePriceApiClient roomTypePriceApiClient)
        {
            _roomManager = roomManager;
            _mapper = mapper;
            _roomTypePriceApiClient = roomTypePriceApiClient;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<RoomDto> rooms = await _roomManager.GetAllWithImagesAsync();

            // Her oda tipinden bir tane al
            List<RoomDto> distinctRooms = rooms
                .GroupBy(r => r.RoomType)
                .Select(g => g.First())
                .ToList();

            List<RoomResponseModel> modelList = _mapper.Map<List<RoomResponseModel>>(distinctRooms);

            // 🔥 WebAPI'den fiyat çek
            foreach (var room in modelList)
            {
                var apiPrice = await _roomTypePriceApiClient.GetPriceByRoomTypeAsync(room.RoomType);
                if (apiPrice.HasValue)
                    room.PricePerNight = apiPrice.Value;
            }

            return View(modelList);
        }

        [HttpGet("Room/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            Room room = await _roomManager.GetFirstOrDefaultAsync(
                predicate: x => x.Id == id,
                include: x => x.Include(r => r.RoomImages)
            );

            if (room == null)
                return NotFound();

            RoomResponseModel model = _mapper.Map<RoomResponseModel>(_mapper.Map<RoomDto>(room));

            // 🔥 Detayda da WebAPI'den fiyat çek
            var apiPrice = await _roomTypePriceApiClient.GetPriceByRoomTypeAsync(model.RoomType);
            if (apiPrice.HasValue)
                model.PricePerNight = apiPrice.Value;

            return View(model);
        }
    }
}
