using AutoMapper;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{  /// <summary>
   /// Oda görsellerini yöneten manager sınıfıdır.
   /// </summary>
    public class RoomImageManager : BaseManager<RoomImageDto, RoomImage>, IRoomImageManager
    {
        private readonly IRoomImageRepository _roomImageRepository;
        private readonly IMapper _mapper;

        public RoomImageManager(IRoomImageRepository roomImageRepository, IMapper mapper)
            : base(roomImageRepository, mapper)
        {
            _roomImageRepository = roomImageRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni bir oda görseli ekler.
        /// </summary>
        public async Task<bool> AddImageAsync(RoomImageDto dto)
        {
            RoomImage entity = _mapper.Map<RoomImage>(dto);
            await _roomImageRepository.AddAsync(entity);
            return true;
        }

        /// <summary>
        /// Belirtilen ID'ye sahip görseli siler.
        /// </summary>
        public async Task<bool> DeleteImageAsync(int id)
        {
            RoomImage? image = await _roomImageRepository.GetByIdAsync(id);
            if (image == null) return false;

            await _roomImageRepository.RemoveAsync(image);
            return true;
        }

        /// <summary>
        /// Belirli bir odaya ait tüm görselleri getirir.
        /// </summary>
        public async Task<List<RoomImageDto>> GetImagesByRoomAsync(int roomId)
        {
            List<RoomImage> list = (await _roomImageRepository.GetAllAsync(x => x.RoomId == roomId)).ToList();
            return _mapper.Map<List<RoomImageDto>>(list);
        }

        /// <summary>
        /// Görsel ID’sine göre tek bir görseli getirir.
        /// </summary>
        public async Task<RoomImageDto> GetImageByIdAsync(int id)
        {
            RoomImage? entity = await _roomImageRepository.GetByIdAsync(id);
            return _mapper.Map<RoomImageDto>(entity);
        }
    }
}
