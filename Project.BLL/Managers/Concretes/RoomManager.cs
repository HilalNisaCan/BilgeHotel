using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.ContextClasses;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows; // MessageBox Kullanımı İçin



namespace Project.BLL.Managers.Concretes
{
    /// <summary>
    /// Oda işlemlerini yöneten manager sınıfıdır.
    /// Sadece MVC tarafında kullanılan metotlar bırakılmıştır.
    /// </summary>
    public class RoomManager : BaseManager<RoomDto, Room>, IRoomManager
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypePriceRepository _roomTypePriceRepository;
        private readonly IMapper _mapper;

        public RoomManager(IRoomRepository roomRepository, IMapper mapper,IRoomTypePriceRepository roomTypePriceRepository):base(roomRepository ,mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _roomTypePriceRepository = roomTypePriceRepository;
        }



        /// <summary>
        /// Tüm odaları ve görsellerini getirir.
        /// Kullanıldığı yer: MVC tarafında oda listeleme sayfası.
        /// </summary>
        public async Task<List<RoomDto>> GetAllWithImagesAsync()
        {
            IEnumerable<Room> rooms = await _roomRepository.GetAllWithIncludeAsync(
                predicate: x => true,
                include: query => query.Include(r => r.RoomImages)
            );

            List<Room> distinctRooms = rooms
                .ToList()
                .GroupBy(r => r.Id)
                .Select(g => g.First())
                .ToList();

            List<RoomDto> roomDtoList = _mapper.Map<List<RoomDto>>(distinctRooms);
            return roomDtoList;
        }


        /// <summary>
        /// Tüm odaları, tiplerine göre fiyat bilgisiyle birlikte getirir.
        /// Kullanıldığı yer: Admin panelde oda fiyat listesi gösterimi.
        /// </summary>
        public async Task<List<RoomDto>> GetAllWithPricesAsync()
        {
            List<Room> rooms = (await _roomRepository.GetAllAsync()).ToList();
            List<RoomDto> roomDtos = _mapper.Map<List<RoomDto>>(rooms);

            foreach (RoomDto dto in roomDtos)
            {
                RoomTypePrice? price = await _roomTypePriceRepository.GetByRoomTypeAsync(dto.RoomType);
                dto.PricePerNight = price?.PricePerNight ?? 0;
            }

            return roomDtos;
        }


        /// <summary>
        /// Boş (müsait) odaları getirir. Görselleri dahil.
        /// Kullanıldığı yer: Web kullanıcı oda seçimi ekranı.
        /// </summary>
        public async Task<List<RoomDto>> GetAvailableRoomsAsync()
        {
            IEnumerable<Room> rooms = await _roomRepository.GetAllWithIncludeAsync(
                x => x.RoomStatus == RoomStatus.Available,
                q => q.Include(r => r.RoomImages)
            );

            return _mapper.Map<List<RoomDto>>(rooms);
        }


        /// <summary>
        /// Belirli bir ID'ye sahip odayı ve görsellerini getirir.
        /// Kullanıldığı yer: Room Detail sayfası.
        /// </summary>
        public async Task<RoomDto> GetByIdWithImagesAsync(int id)
        {
            Room room = await _roomRepository.GetFirstOrDefaultAsync(
                predicate: x => x.Id == id,
                include: query => query.Include(r => r.RoomImages)
            );

            return _mapper.Map<RoomDto>(room);
        }




        /// <summary>
        /// Belirli filtre ve include ile ilk eşleşen odayı getirir.
        /// Kullanıldığı yer: Rezervasyon ekranı (ilk boş oda bulma).
        /// </summary>
        public async Task<Room> GetFirstOrDefaultAsync(
            Expression<Func<Room, bool>> predicate,
            Func<IQueryable<Room>, IQueryable<Room>> include)
        {
            return await _repository.GetFirstOrDefaultAsync(predicate, include);
        }


    }
}
