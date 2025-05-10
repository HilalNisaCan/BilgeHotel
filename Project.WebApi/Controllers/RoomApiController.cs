using Microsoft.AspNetCore.Mvc;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Enums;

namespace Project.WebApi.Controllers
{
    /// <summary>
    /// Oda tipi bazlı fiyat bilgisini sağlayan API controller.
    /// Web sitesi ve mobil rezervasyon işlemleri için kullanılır.
    /// </summary>
    [Route("api/room")]
    [ApiController]
    public class RoomApiController : ControllerBase
    {
        private readonly IRoomTypePriceManager _roomTypePriceManager;

        public RoomApiController(IRoomTypePriceManager roomTypePriceManager)
        {
            _roomTypePriceManager = roomTypePriceManager;
        }

        /// <summary>
        /// Belirtilen oda tipine ait fiyat bilgisini döner.
        /// Örnek: /api/room/price/2
        /// </summary>
        /// <param name="roomType">RoomType enum değeri</param>
        /// <returns>Fiyat bilgisi (decimal)</returns>
        [HttpGet("price/{roomType}")]
        public async Task<IActionResult> GetPrice(RoomType roomType)
        {
            decimal? price = await _roomTypePriceManager.GetPriceByRoomTypeAsync(roomType);

            if (price == null)
                return NotFound("Fiyat bilgisi bulunamadı.");

            return Ok(new { price = price });
        }
    }
}
