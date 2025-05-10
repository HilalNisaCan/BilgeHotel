using Microsoft.AspNetCore.Mvc;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Enums;

namespace Project.WebApi.Controllers
{
    /// <summary>
    /// Oda tiplerine ait fiyatları sağlayan API controller.
    /// Enum değeri string olarak alınıp decimal fiyat dönülür.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypePriceController : ControllerBase
    {
        private readonly IRoomTypePriceManager _roomTypePriceManager;

        public RoomTypePriceController(IRoomTypePriceManager roomTypePriceManager)
        {
            _roomTypePriceManager = roomTypePriceManager;
        }

        /// <summary>
        /// Oda tipi adına göre fiyat bilgisi getirir.
        /// Örnek: /api/RoomTypePrice/price/KingSuite
        /// </summary>
        /// <param name="roomType">RoomType (string olarak gelir)</param>
        /// <returns>decimal fiyat bilgisi</returns>
        [HttpGet("price/{roomType}")]
        public async Task<IActionResult> GetRoomPrice(string roomType)
        {
            // Geçerli bir RoomType değilse hata döner
            if (!Enum.TryParse<RoomType>(roomType, out RoomType type))
                return BadRequest("Oda tipi geçersiz.");

            // Fiyatı getir
            decimal price = await _roomTypePriceManager.GetPriceByRoomTypeAsync(type);

            return Ok(new { price = price });
        }
    }
}
