using AutoMapper;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    /// <summary>
    /// Ürün işlemlerini yöneten manager sınıfıdır.
    /// </summary>
    public class ProductManager : BaseManager<ProductDto, Product>, IProductManager
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductManager(IProductRepository productRepository, IMapper mapper)
            : base(productRepository, mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetByCategoryAsync(ProductCategory category)
        {
            List<Product> products = (List<Product>)await _productRepository.GetAllAsync(p => p.Category == category);
            return _mapper.Map<List<ProductDto>>(products);
        }



      
    }
}