using Microsoft.AspNetCore.Mvc;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Enums;

namespace Project.WebApi.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomApiController : ControllerBase
    {
        private readonly IRoomTypePriceManager _roomTypePriceManager;

        public RoomApiController(IRoomTypePriceManager roomTypePriceManager)
        {
            _roomTypePriceManager = roomTypePriceManager;
        }

        [HttpGet("price/{roomType}")]
        public async Task<IActionResult> GetPrice(RoomType roomType)
        {
            decimal? price = await _roomTypePriceManager.GetPriceByRoomTypeAsync(roomType);
            if (price == null)
                return NotFound();

            return Ok(new { price });
        }
    }
}
