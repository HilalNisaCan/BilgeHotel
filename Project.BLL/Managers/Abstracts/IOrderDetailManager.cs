using Project.BLL.DtoClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    /// <summary>
    /// Sipariş detaylarıyla ilgili iş akışlarını tanımlar.
    /// </summary>
    public interface IOrderDetailManager : IManager<OrderDetailDto, OrderDetail>
    {
        /// <summary>
        /// Yeni bir sipariş detay kaydı ekler.
        /// </summary>
        Task<bool> AddOrderDetailAsync(OrderDetailDto dto);

        /// <summary>
        /// Mevcut bir sipariş detayını günceller.
        /// </summary>
        Task<bool> UpdateOrderDetailAsync(OrderDetailDto dto);

        /// <summary>
        /// Belirli bir siparişe ait tüm detayları getirir.
        /// </summary>
        Task<List<OrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId);

        /// <summary>
        /// Sipariş detayını sistemden kaldırır.
        /// </summary>
        Task<bool> DeleteOrderDetailAsync(int id);
    }
}
