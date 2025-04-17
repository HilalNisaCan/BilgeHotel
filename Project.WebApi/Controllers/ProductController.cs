using Microsoft.AspNetCore.Mvc;
using Project.BLL.Managers.Abstracts;
using Project.Entities.Enums;

namespace Project.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        // 🔥 API: /api/product/byCategory/2 gibi çalışır
        [HttpGet("byCategory/{category}")]
        public async Task<IActionResult> GetProductsByCategory(int category)
        {
            var products = await _productManager.GetByCategoryAsync((ProductCategory)category);

            var simplified = products.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                unitPrice = p.Price,
            });

            return Ok(simplified);
        }
    }
}
