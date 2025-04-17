using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.Repositories.Abstracts
{
    public interface IRoomImageRepository:IRepository<RoomImage>
    {
        Task<List<RoomImage>> GetImagesByRoomIdAsync(int roomId); // Belirli bir odanın tüm fotoğraflarını getir
        Task RemoveAllImagesByRoomIdAsync(int roomId); // Belirli bir odanın tüm fotoğraflarını sil
    }
}
