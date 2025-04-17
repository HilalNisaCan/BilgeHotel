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

        /// <summary>
        /// Yeni bir ürün ekler ve ID döner.
        /// </summary>
        public async Task<int> AddProductAsync(ProductDto dto)
        {
            var entity = _mapper.Map<Product>(dto);
            await _productRepository.AddAsync(entity);
            return entity.Id;
        }

        /// <summary>
        /// Mevcut bir ürünü günceller.
        /// </summary>
        public async Task<bool> UpdateProductAsync(ProductDto dto)
        {
            var entity = await _productRepository.GetByIdAsync(dto.Id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _productRepository.UpdateAsync(entity);
            return true;
        }

        /// <summary>
        /// Belirli bir ürün ID'sine göre ürün bilgisini getirir.
        /// </summary>
        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var entity = await _productRepository.GetByIdAsync(id);
            return _mapper.Map<ProductDto>(entity);
        }

        /// <summary>
        /// Sistemdeki tüm ürünleri getirir.
        /// </summary>
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var list = await _productRepository.GetAllAsync();
            return _mapper.Map<List<ProductDto>>(list);
        }

        /// <summary>
        /// Belirli bir kategoriye ait ürünleri getirir.
        /// </summary>
        public async Task<List<ProductDto>> GetProductsByCategoryAsync(ProductCategory category)
        {
            var list = await _productRepository.GetAllAsync(x => x.Category == category);
            return _mapper.Map<List<ProductDto>>(list);
        }

        /// <summary>
        /// Belirli bir ürünü sistemden kaldırır.
        /// </summary>
        public async Task<bool> DeleteProductAsync(int id)
        {
            var entity = await _productRepository.GetByIdAsync(id);
            if (entity == null) return false;

            await _productRepository.RemoveAsync(entity);
            return true;

        }

        public async Task<List<ProductDto>> GetByCategoryAsync(ProductCategory category)
        {
            List<Product> products = (await _productRepository.GetAllAsync(p => p.Category == category)).ToList();
            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}