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
    /// Oda görsellerinin yönetimi için iş akışlarını tanımlar.
    /// </summary>
    public interface IRoomImageManager : IManager<RoomImageDto, RoomImage>
    {
        /// <summary>
        /// Yeni bir oda görseli ekler.
        /// </summary>
        Task<bool> AddImageAsync(RoomImageDto dto);

        /// <summary>
        /// Belirtilen görseli siler.
        /// </summary>
        Task<bool> DeleteImageAsync(int id);

        /// <summary>
        /// Belirli bir odaya ait tüm görselleri getirir.
        /// </summary>
        Task<List<RoomImageDto>> GetImagesByRoomAsync(int roomId);

        /// <summary>
        /// Görsel ID'sine göre tek bir görseli getirir.
        /// </summary>
        Task<RoomImageDto> GetImageByIdAsync(int id);
    }
}
