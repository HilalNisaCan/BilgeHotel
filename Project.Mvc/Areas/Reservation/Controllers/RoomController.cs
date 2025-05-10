using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Enums;
using Project.MvcUI.Models.PureVm.ResponseModel.Room;

namespace Project.MvcUI.Areas.Reservation.Controllers
{

    /*“Reservation Area’da yer alan RoomController, yalnızca müsait durumdaki odaları listelemek amacıyla hazırlanmıştır.
     * DTO → ViewModel dönüşümleri AutoMapper ile sağlanır. Kullanıcı dostu sade arayüz üzerinden resepsiyonistler
     * odaların doluluk durumunu anlık olarak görüntüleyebilir. İş kurallarına uygun olarak sadece
     * boş durumdaki odalar filtrelenerek listelenir.”*/

    [Area("Reservation")]
    [Route("Reservation/Rooms")]
    public class RoomController : Controller
    {
        private readonly IRoomManager _roomManager;
        private readonly IMapper _mapper;

        public RoomController(IRoomManager roomManager, IMapper mapper)
        {
            _roomManager = roomManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Müsait (Available) odaları listeler
        /// </summary>
        [HttpGet("Available")]
        public async Task<IActionResult> Available()
        {
            // Tüm odaları fiyat bilgisiyle birlikte çekiyoruz
            List<RoomDto> roomsWithPrices = await _roomManager.GetAllWithPricesAsync();

            // Sadece müsait (Available) olanları filtreliyoruz
            List<RoomDto> availableRooms = roomsWithPrices
                .Where(x => x.Status == RoomStatus.Available)
                .ToList();

            // ViewModel'e çeviriyoruz
            List<RoomResponseModel> model = _mapper.Map<List<RoomResponseModel>>(availableRooms);

            return View(model);
        }
    }
}
