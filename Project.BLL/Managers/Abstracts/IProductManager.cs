using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    /// <summary>
    /// Ürün yönetimi iş akışlarını tanımlar.
    /// </summary>
    public interface IProductManager : IManager<ProductDto, Product>
    {
        /// <summary>
        /// Yeni bir ürün ekler ve ürün ID'sini döner.
        /// </summary>
        Task<int> AddProductAsync(ProductDto dto);

        /// <summary>
        /// Mevcut bir ürünü günceller.
        /// </summary>
        Task<bool> UpdateProductAsync(ProductDto dto);

        /// <summary>
        /// Belirli bir ürün ID'sine göre ürünü getirir.
        /// </summary>
        Task<ProductDto> GetProductByIdAsync(int id);

        /// <summary>
        /// Tüm ürünleri getirir.
        /// </summary>
        Task<List<ProductDto>> GetAllProductsAsync();

        /// <summary>
        /// Belirli bir kategoriye ait ürünleri getirir.
        /// </summary>
        Task<List<ProductDto>> GetProductsByCategoryAsync(ProductCategory category);

        /// <summary>
        /// Belirli bir ürünü sistemden kaldırır.
        /// </summary>
        Task<bool> DeleteProductAsync(int id);

        Task<List<ProductDto>> GetByCategoryAsync(ProductCategory category);
    }
}
