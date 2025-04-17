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
    public class RoomManager : BaseManager<RoomDto, Room>, IRoomManager
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTypePriceRepository _roomTypePriceRepository;
        private readonly IRoomCleaningScheduleRepository _cleaningRepository;
        private readonly IRoomMaintenanceRepository _maintenanceRepository;
        private readonly IMapper _mapper;

        public RoomManager(IRoomRepository roomRepository, IRoomCleaningScheduleRepository cleaningRepository,IMapper mapper,IRoomMaintenanceRepository maintenanceRepository,IRoomTypePriceRepository roomTypePriceRepository):base(roomRepository ,mapper)
        {
            _roomRepository = roomRepository;
            _cleaningRepository = cleaningRepository;
            _maintenanceRepository = maintenanceRepository;
            _mapper = mapper;
            _roomTypePriceRepository = roomTypePriceRepository;
        }


        /// <summary>
        /// Odanın durumunu değiştirir (örneğin: Boş, Dolu, Temizlikte).
        /// </summary>
        public async Task ChangeRoomStatusAsync(int roomId, RoomStatus status)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null) throw new Exception("Oda bulunamadı.");

            room.RoomStatus = status;
            await _roomRepository.UpdateAsync(room);
        }

        /// <summary>
        /// Tüm odaları ilişkili görselleriyle birlikte getirir.
        /// </summary>
        public async Task<List<RoomDto>> GetAllWithImagesAsync()
        {
            var rooms = await _roomRepository.GetAllWithIncludeAsync(
        predicate: x => true,
        include: query => query.Include(r => r.RoomImages)
    );

            // ToList zorunlu, sonra GroupBy yapılabilir
            var distinctRooms = rooms
                .ToList()
                .GroupBy(r => r.Id)
                .Select(g => g.First())
                .ToList();

            var roomDtoList = _mapper.Map<List<RoomDto>>(distinctRooms);
            return roomDtoList;
        }

        public  async Task<List<RoomDto>> GetAllWithPricesAsync()
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
        /// Şu anda boş olan odaları getirir.
        /// </summary>
        public async Task<List<RoomDto>> GetAvailableRoomsAsync()
        {
            IEnumerable<Room> rooms = await _roomRepository.GetAllWithIncludeAsync(
            x => x.RoomStatus == RoomStatus.Available,
            q => q.Include(r => r.RoomImages) // Eğer RoomImage ilişkisi varsa
            );

            return _mapper.Map<List<RoomDto>>(rooms);
        }

        public async Task<RoomDto> GetByIdWithImagesAsync(int id)
        {
            Room room = await _roomRepository.GetFirstOrDefaultAsync(
                predicate: x => x.Id == id,
                include: query => query.Include(r => r.RoomImages)
            );

            return _mapper.Map<RoomDto>(room);
        }

        public async Task<RoomDto> GetByIdWithPriceAsync(int RoomId)
        {
            Room? room = await _roomRepository.GetByIdAsync(RoomId);

            if (room == null)
                return null;

            RoomTypePrice? price = await _roomTypePriceRepository.GetByRoomTypeAsync(room.RoomType);

            RoomDto dto = _mapper.Map<RoomDto>(room);
            dto.PricePerNight = price != null ? price.PricePerNight : 0;

            return dto;
        }

        public async Task<Room> GetFirstAvailableRoomByTypeAsync(RoomType roomType)
        {
            return await GetFirstOrDefaultAsync(
                r => r.RoomType == roomType && r.RoomStatus == RoomStatus.Available,
                include => include.Include(r => r.RoomImages));
        }

        public async Task<Room> GetFirstOrDefaultAsync(Expression<Func<Room, bool>> predicate, Func<IQueryable<Room>, IQueryable<Room>> include)
        {
            return await _repository.GetFirstOrDefaultAsync(predicate, include);
        }

        /// <summary>
        /// Belirli bir odanın tüm detaylarını getirir.
        /// </summary>
        public async Task<Room> GetRoomDetailsAsync(int roomId)
        {
            return await _roomRepository.GetByIdAsync(roomId) ?? throw new Exception("Oda bulunamadı.");
        }

        public async Task<decimal> GetRoomPriceAsync(RoomType roomType)
        {
            RoomTypePrice? priceEntity = await _roomTypePriceRepository
           .GetFirstOrDefaultAsync(
            p => p.RoomType == roomType,
            query => query // include yerine boş bırak
            );
            return priceEntity?.PricePerNight ?? 0;
        }

        /// <summary>
        /// Odanın temizlik işlemi tamamlandığında temiz olarak işaretler.
        /// </summary>
        public async Task MarkRoomAsCleanedAsync(int roomId)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null) throw new Exception("Oda bulunamadı.");

            room.IsCleaned = true;
            room.RoomStatus = RoomStatus.Available;
            await _roomRepository.UpdateAsync(room);
        }

        /// <summary>
        /// Odayı temizliğe planlar ve temizlik zamanlamasını oluşturur.
        /// </summary>
        public async Task ScheduleCleaningAsync(int roomId, DateTime cleaningDate)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null) throw new Exception("Oda bulunamadı.");

            room.RoomStatus = RoomStatus.Cleaning;
            await _roomRepository.UpdateAsync(room);

            var cleaningSchedule = new RoomCleaningSchedule
            {
                RoomId = roomId,
                ScheduledDate = cleaningDate,
                CleaningStatus = CleaningStatus.Scheduled
            };

            await _cleaningRepository.AddAsync(cleaningSchedule);
        }

        /// <summary>
        /// Oda için bakım zamanlaması oluşturur.
        /// </summary>
        public async Task ScheduleMaintenanceAsync(int roomId, DateTime maintenanceDate, MaintenanceType type)
        {
            var maintenance = new RoomMaintenance
            {
                RoomId = roomId,
                ScheduledDate = maintenanceDate,
                MaintenanceType = type,
                MaintenanceStatus = MaintenanceStatus.Pending
            };

            await _maintenanceRepository.AddAsync(maintenance);
        }
    }
}
