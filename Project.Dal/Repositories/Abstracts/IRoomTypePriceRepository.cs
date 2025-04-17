using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IRoomTypePriceRepository : IRepository<RoomTypePrice>
    {
        Task SaveAsync(); // Bu metodu ekle

        Task<RoomTypePrice?> GetByRoomTypeAsync(RoomType roomType);
    }
}
