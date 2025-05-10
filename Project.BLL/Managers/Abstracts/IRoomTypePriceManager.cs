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
    /// <summary>
    /// Oda tipi bazlı fiyat işlemlerini yöneten arayüz.
    /// </summary>
    public interface IRoomTypePriceManager : IManager<RoomTypePriceDto, RoomTypePrice>
    {
        /// <summary>
        /// Yeni oda fiyatı oluşturur.
        /// </summary>
        Task<RoomTypePriceDto> CreateRoomTypePriceAsync(RoomTypePriceDto model);

        /// <summary>
        /// Tüm oda tipi fiyatlarını getirir.
        /// </summary>
        Task<List<RoomTypePriceDto>> GetAllRoomTypePricesAsync();

        /// <summary>
        /// Mevcut fiyatı günceller.
        /// </summary>
        Task UpdateRoomTypePriceAsync(int id, RoomTypePriceDto model);

        /// <summary>
        /// Oda fiyatını siler.
        /// </summary>
        Task DeleteRoomTypePriceAsync(int id);

        /// <summary>
        /// Oda tipine göre fiyat bilgisini getirir.
        /// </summary>
        Task<RoomTypePriceDto> GetByRoomTypeAsync(RoomType roomType);

        /// <summary>
        /// Sadece fiyat değerini getirir.
        /// </summary>
        Task<decimal> GetPriceByRoomTypeAsync(RoomType roomType);
    }
}
