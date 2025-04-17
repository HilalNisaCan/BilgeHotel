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

        // Yeni fiyat ekler
        public async Task<RoomTypePriceDto> CreateRoomTypePriceAsync(RoomTypePriceDto model)
        {
            // ❗ Aynı RoomType varsa ekleme
            RoomTypePrice? existing = await _repository.GetFirstOrDefaultAsync(
            x => x.RoomType == model.RoomType,
            x => x // ❗ boş include zorunluysa bu şekilde gönderilir
            );

            if (existing != null)
                return null; // ❌ zaten kayıtlı, ikinci kez eklenemez

            RoomTypePrice roomTypePrice = new RoomTypePrice
            {
                RoomType = model.RoomType,
                PricePerNight = model.PricePerNight
            };

            await _repository.AddAsync(roomTypePrice);
            await _repository.SaveAsync();

            return _mapper.Map<RoomTypePriceDto>(roomTypePrice);
        }

        // RoomTypePrice'ı siler
        public async Task DeleteRoomTypePriceAsync(int id)
        {
            RoomTypePrice roomTypePrice = await _repository.GetByIdAsync(id);
            if (roomTypePrice != null)
            {
                await _repository.RemoveAsync(roomTypePrice);
                await _repository.SaveAsync();
            }
        }

        // RoomTypePrice'leri getirir
        public async Task<List<RoomTypePriceDto>> GetAllRoomTypePricesAsync()
        {
            List<RoomTypePrice> roomTypePrices = (List<RoomTypePrice>)await _repository.GetAllAsync();
            return _mapper.Map<List<RoomTypePriceDto>>(roomTypePrices);
        }

        public async Task<RoomTypePriceDto> GetByRoomTypeAsync(RoomType roomType)
        {
            var roomTypePrice = await _repository.GetAllAsync();
            var result = roomTypePrice.FirstOrDefault(x => x.RoomType == roomType);

            return _mapper.Map<RoomTypePriceDto>(result); 
        }

        public async Task<decimal> GetPriceByRoomTypeAsync(RoomType roomType)
        {
            // Veriyi getirmek için include parametresini kullan
            var roomTypePrice = await _repository.FirstOrDefaultAsync(x => x.RoomType == roomType);

            if (roomTypePrice == null)
                return 0; // Null yerine 0 döndürüyoruz

            return roomTypePrice.PricePerNight; // PricePerNight'ı döndür
        }

        // Fiyatı günceller
        public async Task UpdateRoomTypePriceAsync(int id, RoomTypePriceDto model)
        {
            RoomTypePrice roomTypePrice = await _repository.GetByIdAsync(id);
            if (roomTypePrice != null)
            {
                roomTypePrice.RoomType = model.RoomType;
                roomTypePrice.PricePerNight = model.PricePerNight;

                await _repository.SaveAsync();
            }
        }
    }
}
