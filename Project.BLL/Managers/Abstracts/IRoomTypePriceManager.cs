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
    public interface IRoomTypePriceManager:IManager<RoomTypePriceDto,RoomTypePrice>
    {
        Task<RoomTypePriceDto> CreateRoomTypePriceAsync(RoomTypePriceDto model);

        Task<List<RoomTypePriceDto>> GetAllRoomTypePricesAsync();

        Task UpdateRoomTypePriceAsync(int id, RoomTypePriceDto model);

        Task DeleteRoomTypePriceAsync(int id);


        Task<RoomTypePriceDto> GetByRoomTypeAsync(RoomType roomType);

        Task<decimal> GetPriceByRoomTypeAsync(RoomType roomType);


    }
}
