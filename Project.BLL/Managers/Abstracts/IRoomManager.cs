using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Abstracts
{
    public interface IRoomManager : IManager<RoomDto, Room>
    {
        /// <summary>
        /// Şu anda boş olan tüm odaları getirir.
        /// </summary>
        Task<List<RoomDto>> GetAvailableRoomsAsync();

        /// <summary>
        /// Belirtilen odanın durumunu değiştirir.
        /// </summary>
        Task ChangeRoomStatusAsync(int roomId, RoomStatus status);

        /// <summary>
        /// Odayı belirlenen tarihte temizliğe planlar.
        /// </summary>
        Task ScheduleCleaningAsync(int roomId, DateTime cleaningDate);

        /// <summary>
        /// Odanın temizlik işlemi tamamlandığında temiz olarak işaretler.
        /// </summary>
        Task MarkRoomAsCleanedAsync(int roomId);

        /// <summary>
        /// Belirli bir odada bakım planlar.
        /// </summary>
        Task ScheduleMaintenanceAsync(int roomId, DateTime maintenanceDate, MaintenanceType type);

        /// <summary>
        /// Belirli bir odanın tüm detaylarını getirir.
        /// </summary>
        Task<Room> GetRoomDetailsAsync(int roomId);

        Task<Room>  GetFirstOrDefaultAsync(Expression<Func<Room, bool>> predicate, Func<IQueryable<Room>, IQueryable<Room>> include);

        Task<List<RoomDto>> GetAllWithImagesAsync();

        Task<Room> GetFirstAvailableRoomByTypeAsync(RoomType roomType);

        // IRoomManager.cs
        Task<decimal> GetRoomPriceAsync(RoomType roomType);

        Task<RoomDto> GetByIdWithImagesAsync(int id);

        Task<RoomDto> GetByIdWithPriceAsync(int RoomId);

        /// <summary>
        /// Tüm odaları fiyat bilgileriyle birlikte getirir.
        /// </summary>
        Task<List<RoomDto>> GetAllWithPricesAsync();
    }
}
