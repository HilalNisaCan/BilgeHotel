using AutoMapper;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    /// <summary>
    /// Sipariş detaylarını yöneten manager sınıfı.
    /// </summary>
    public class OrderDetailManager : BaseManager<OrderDetailDto, OrderDetail>, IOrderDetailManager
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderDetailManager(
            IOrderDetailRepository orderDetailRepository,
            IOrderRepository orderRepository,
            IMapper mapper)
            : base(orderDetailRepository, mapper)
        {
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni bir sipariş detay kaydı ekler.
        /// Sipariş var mı kontrol edilerek işlem yapılır.
        /// </summary>
        public async Task<bool> AddOrderDetailAsync(OrderDetailDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(dto.OrderId);
            if (order == null)
                return false;

            var entity = _mapper.Map<OrderDetail>(dto);
            await _orderDetailRepository.AddAsync(entity);
            return true;
        }

        /// <summary>
        /// Mevcut bir sipariş detayını günceller.
        /// </summary>
        public async Task<bool> UpdateOrderDetailAsync(OrderDetailDto dto)
        {
            var entity = await _orderDetailRepository.GetByIdAsync(dto.Id);
            if (entity == null)
                return false;

            _mapper.Map(dto, entity);
            await _orderDetailRepository.UpdateAsync(entity);
            return true;
        }

        /// <summary>
        /// Belirli bir siparişe ait tüm sipariş detaylarını getirir.
        /// </summary>
        public async Task<List<OrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            var list = await _orderDetailRepository.GetAllAsync(x => x.OrderId == orderId);
            return _mapper.Map<List<OrderDetailDto>>(list);
        }

        /// <summary>
        /// Sipariş detay kaydını sistemden kaldırır.
        /// </summary>
        public async Task<bool> DeleteOrderDetailAsync(int id)
        {
            var entity = await _orderDetailRepository.GetByIdAsync(id);
            if (entity == null)
                return false;

            await _orderDetailRepository.RemoveAsync(entity);
            return true;
        }
    }
}
