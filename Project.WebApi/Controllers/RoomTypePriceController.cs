using Microsoft.AspNetCore.Mvc;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Enums;

namespace Project.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypePriceController : ControllerBase
    {
        private readonly IRoomTypePriceManager _roomTypePriceManager;

        public RoomTypePriceController(IRoomTypePriceManager roomTypePriceManager)
        {
            _roomTypePriceManager = roomTypePriceManager;
        }

        [HttpGet("price/{roomType}")]
        public async Task<IActionResult> GetRoomPrice(string roomType)
        {
            if (!Enum.TryParse<RoomType>(roomType, out RoomType type))
                return BadRequest("Oda tipi geçersiz.");

            decimal price = await _roomTypePriceManager.GetPriceByRoomTypeAsync(type);
            return Ok(new { price });
        }
    }
}
