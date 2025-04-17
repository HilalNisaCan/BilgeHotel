using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Concretes
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(MyContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(ProductCategory category)
        {
            return await _dbSet.Where(p => p.Category == category).ToListAsync();
        }

        public async Task<bool> IsProductInStockAsync(int productId)
        {
            return await _dbSet.AnyAsync(p => p.Id == productId && p.IsInStock);
        }

        public async Task<List<Product>> GetPopularProductsAsync(int topCount)
        {
            return await _dbSet.OrderByDescending(p => p.IsInStock).Take(topCount).ToListAsync();
        }
    }
}
