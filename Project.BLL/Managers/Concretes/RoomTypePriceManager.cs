using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    public class RoomTypePriceManager : BaseManager<RoomTypePriceDto, RoomTypePrice>, IRoomTypePriceManager
    {
        private readonly IRoomTypePriceRepository _repository;
        private readonly IMapper _mapper;

        public RoomTypePriceManager(IRoomTypePriceRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni bir oda tipi fiyatı ekler. Aynı RoomType varsa tekrar eklemez.
        /// </summary>
        public async Task<RoomTypePriceDto> CreateRoomTypePriceAsync(RoomTypePriceDto model)
        {
            RoomTypePrice? existing = await _repository.GetFirstOrDefaultAsync(
                x => x.RoomType == model.RoomType,
                x => x // include kullanılmazsa boş geçilebilir
            );

            if (existing != null)
                return null;

            RoomTypePrice roomTypePrice = new RoomTypePrice
            {
                RoomType = model.RoomType,
                PricePerNight = model.PricePerNight
            };

            await _repository.AddAsync(roomTypePrice);
            await _repository.SaveAsync();

            return _mapper.Map<RoomTypePriceDto>(roomTypePrice);
        }


        /// <summary>
        /// Oda tipi fiyatını siler.
        /// </summary>
        public async Task DeleteRoomTypePriceAsync(int id)
        {
            RoomTypePrice? roomTypePrice = await _repository.GetByIdAsync(id);
            if (roomTypePrice != null)
            {
                await _repository.RemoveAsync(roomTypePrice);
                await _repository.SaveAsync();
            }
        }

        /// <summary>
        /// Tüm oda tipi fiyatlarını listeler.
        /// </summary>
        public async Task<List<RoomTypePriceDto>> GetAllRoomTypePricesAsync()
        {
            IEnumerable<RoomTypePrice> roomTypePrices = await _repository.GetAllAsync();
            return _mapper.Map<List<RoomTypePriceDto>>(roomTypePrices.ToList());
        }

        /// <summary>
        /// Belirli bir oda tipine ait fiyat bilgisini getirir (DTO olarak).
        /// </summary>
        public async Task<RoomTypePriceDto> GetByRoomTypeAsync(RoomType roomType)
        {
            IEnumerable<RoomTypePrice> roomTypePrices = await _repository.GetAllAsync();
            RoomTypePrice? result = roomTypePrices.FirstOrDefault(x => x.RoomType == roomType);

            return _mapper.Map<RoomTypePriceDto>(result);
        }

        /// <summary>
        /// Belirli bir oda tipine ait gecelik fiyatı döner.
        /// </summary>
        public async Task<decimal> GetPriceByRoomTypeAsync(RoomType roomType)
        {
            RoomTypePrice? roomTypePrice = await _repository.FirstOrDefaultAsync(x => x.RoomType == roomType);

            if (roomTypePrice == null)
                return 0;

            return roomTypePrice.PricePerNight;
        }


        /// <summary>
        /// Var olan bir oda tipine ait fiyat bilgisini günceller.
        /// </summary>
        public async Task UpdateRoomTypePriceAsync(int id, RoomTypePriceDto model)
        {
            RoomTypePrice? roomTypePrice = await _repository.GetByIdAsync(id);
            if (roomTypePrice != null)
            {
                roomTypePrice.RoomType = model.RoomType;
                roomTypePrice.PricePerNight = model.PricePerNight;

                await _repository.SaveAsync();
            }
        }
    }
}
