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
    public interface IOrderManager : IManager<OrderDto, Order>
    {
        /// <summary>
        /// Yeni sipariş ve detaylarını ekler.
        /// </summary>
        Task<int> AddOrderAsync(int reservationId, List<OrderDetailDto> orderDetails);

        /// <summary>
        /// Sipariş durumunu günceller.
        /// </summary>
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);

        /// <summary>
        /// Belirli bir rezervasyona ait siparişleri getirir.
        /// </summary>
        Task<List<OrderDto>> GetOrdersByReservationAsync(int reservationId);

        /// <summary>
        /// Siparişi ve detaylarını siler.
        /// </summary>
        Task<bool> DeleteOrderAsync(int orderId);
    }
}
