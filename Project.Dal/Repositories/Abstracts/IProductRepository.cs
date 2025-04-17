using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IProductRepository:IRepository<Product>
    {
        Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category); // Ürünleri kategoriye göre getir
        Task<bool> IsProductInStockAsync(int productId); // Ürünün stokta olup olmadığını kontrol et
        Task<List<Product>> GetPopularProductsAsync(int topCount); // En popüler ürünleri getir
    }
}
