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
        /// Kullanıldığı yer: Web/MVC kullanıcı oda seçimi ekranı.
        /// </summary>
        Task<List<RoomDto>> GetAvailableRoomsAsync();

        /// <summary>
        /// Belirli koşula göre ilk odayı getirir. İlişkili veriler include ile alınabilir.
        /// Kullanıldığı yer: Rezervasyon ekranında ilk uygun oda bulmak.
        /// </summary>
        Task<Room> GetFirstOrDefaultAsync(
            Expression<Func<Room, bool>> predicate,
            Func<IQueryable<Room>, IQueryable<Room>> include);

        /// <summary>
        /// Tüm odaları ve onların görsellerini getirir.
        /// Kullanıldığı yer: Ana sayfa / oda listesi görsel galeri.
        /// </summary>
        Task<List<RoomDto>> GetAllWithImagesAsync();

        /// <summary>
        /// ID ile odanın tüm bilgilerini ve görsellerini getirir.
        /// Kullanıldığı yer: Oda detay sayfası.
        /// </summary>
        Task<RoomDto> GetByIdWithImagesAsync(int id);

        /// <summary>
        /// Tüm odaları fiyat bilgileriyle birlikte getirir.
        /// Kullanıldığı yer: Admin fiyat yönetimi, kullanıcı listeleme ekranı.
        /// </summary>
        Task<List<RoomDto>> GetAllWithPricesAsync();
    }
}
