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
        /// Kategoriye göre ürünleri getirir. (Alternatif versiyon)
        /// </summary>
        Task<List<ProductDto>> GetByCategoryAsync(ProductCategory category);
    }
}
