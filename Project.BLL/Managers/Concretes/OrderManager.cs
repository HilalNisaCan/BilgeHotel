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
    public class OrderManager : BaseManager<OrderDto, Order>, IOrderManager
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public OrderManager(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository,
                            IReservationRepository reservationRepository, IMapper mapper)
            : base(orderRepository, mapper)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }


        /// <summary>
        /// Belirli bir rezervasyona ait siparişi ve detaylarını oluşturur.
        /// </summary>
        public async Task<int> AddOrderAsync(int reservationId, List<OrderDetailDto> orderDetails)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null)
                throw new Exception("Rezervasyon bulunamadı!");

            var order = new Order
            {
                ReservationId = reservationId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Preparing
            };

            await _orderRepository.AddAsync(order);

            foreach (var detail in orderDetails)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                };
                await _orderDetailRepository.AddAsync(orderDetail);
            }

            return order.Id;
        }

        /// <summary>
        /// Belirli bir siparişin tüm detaylarıyla birlikte silinmesini sağlar.
        /// </summary>
        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return false;

            var orderDetails = await _orderDetailRepository.GetAllAsync(od => od.OrderId == orderId);
            foreach (var detail in orderDetails)
            {
                await _orderDetailRepository.RemoveAsync(detail);
            }

            await _orderRepository.RemoveAsync(order);
            return true;
        }

        /// <summary>
        /// Belirli bir rezervasyona ait tüm siparişleri getirir.
        /// </summary>
        public async Task<List<OrderDto>> GetOrdersByReservationAsync(int reservationId)
        {
            var orders = await _orderRepository.GetAllAsync(o => o.ReservationId == reservationId);
            return _mapper.Map<List<OrderDto>>(orders);
        }

        /// <summary>
        /// Belirli bir siparişin durumunu günceller.
        /// </summary>
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return false;

            order.Status = (DataStatus)status;
            await _orderRepository.UpdateAsync(order);

            return true;
        }
    }
}
