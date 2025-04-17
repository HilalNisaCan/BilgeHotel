using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<List<Room>> GetAvailableRoomsAsync(DateTime checkInDate, DateTime checkOutDate); // Belirtilen tarihlerde müsait odaları getir
        Task<Room> GetRoomWithImagesAsync(int roomId); // Oda bilgilerini ve fotoğraflarını getir
        Task<List<Room>> GetRoomsByTypeAsync(RoomType roomType); // Oda tipine göre odaları getir
     

    }
}
