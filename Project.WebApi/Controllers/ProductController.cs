using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Enums;

namespace Project.WebApi.Controllers
{
    //"Mobil tarafa sade ürün listesi döndürmek için kullanıyoruz

    /// <summary>
    /// Ürün bilgilerini sağlayan API controller'dır.
    /// Genelde menü sistemi gibi mobil/ekran tarafına veri sağlamak için kullanılır.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        /// <summary>
        /// Belirli bir kategoriye ait ürünleri getirir.
        /// Örnek: /api/product/byCategory/2
        /// </summary>
        /// <param name="category">ProductCategory enum değeri (int olarak gelir)</param>
        /// <returns>Sadeleştirilmiş ürün listesi</returns>
        [HttpGet("byCategory/{category}")]
        public async Task<IActionResult> GetProductsByCategory(int category)
        {
            List<ProductDto> products = await _productManager.GetByCategoryAsync((ProductCategory)category);

            List<object> simplified = products.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                unitPrice = p.Price
            }).Cast<object>().ToList();

            return Ok(simplified);
        }
    }
}
