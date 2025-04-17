using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IOrderDetailRepository:IRepository<OrderDetail>
    {
        Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId); // Belirli bir siparişin detaylarını getir
        Task<List<OrderDetail>> GetOrderDetailsByProductIdAsync(int productId); // Belirli bir ürüne ait siparişleri getir
    }
}
