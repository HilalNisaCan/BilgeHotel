using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    public class ReservationManager : BaseManager<ReservationDto, Reservation>, IReservationManager
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRoomRepository  _roomRepository;
        private readonly IEarlyReservationDiscountManager _discountService; // ✅ Erken rezervasyon indirimi için
        private new readonly IMapper _mapper;

       
        public ReservationManager(IReservationRepository reservationRepo, ICustomerRepository customerRepo, IRoomRepository  roomRepo, IMapper mapper, IEarlyReservationDiscountManager discountService) : base(reservationRepo, mapper)
        {
            _reservationRepository = reservationRepo;
            _customerRepository = customerRepo;
            _roomRepository = roomRepo;
            _mapper = mapper;
            _discountService = discountService;
        }

        /// <summary>
        /// Verilen oda ID'si ile gecelik fiyatı döner (kapsam genişletilebilir).
        /// </summary>
        public async Task<decimal> CalculateTotalPriceAsync(int reservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null) return 0;

            int duration = (int)(reservation.EndDate - reservation.StartDate).TotalDays;
            var room = await _roomRepository.GetByIdAsync(reservation.RoomId);
            return room.PricePerNight * duration;
        }

        /// <summary>
        /// Belirtilen müşterinin geçmiş rezervasyonlarını DTO olarak getirir.
        /// </summary>
        public async Task<List<ReservationDto>> GetCustomerReservationsAsync(int customerId)
        {
            var reservations = await _reservationRepository.GetAllAsync(r => r.CustomerId == customerId);
            return _mapper.Map<List<ReservationDto>>(reservations.OrderByDescending(r => r.StartDate).ToList());
        }

        /// <summary>
        /// Rezervasyon iptalini gerçekleştirir ve oda durumunu günceller.
        /// </summary>
        public async Task CancelReservationAsync(int reservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null || reservation.ReservationStatus == ReservationStatus.Cancelled)
                return;

            reservation.ReservationStatus = ReservationStatus.Cancelled;
            await _reservationRepository.UpdateAsync(reservation);

            var room = await _roomRepository.GetByIdAsync(reservation.RoomId);
            if (room != null)
            {
                room.RoomStatus = RoomStatus.Available;
                await _roomRepository.UpdateAsync(room);
            }
        }

        /// <summary>
        /// Oda belirli tarihler arasında müsait mi kontrol eder.
        /// </summary>
        public async Task<bool> CheckAvailabilityAsync(int roomId, DateTime startDate, int duration)
        {
            var existingReservations = await _reservationRepository.GetAllAsync(r =>
                r.RoomId == roomId &&
                r.ReservationStatus != ReservationStatus.Cancelled &&
                ((r.StartDate <= startDate && r.EndDate > startDate) ||
                 (r.StartDate < startDate.AddDays(duration) && r.EndDate >= startDate.AddDays(duration)))
            );

            return !existingReservations.Any();
        }

        /// <summary>
        /// Rezervasyon oluşturur, indirim uygular ve odayı rezerve eder.
        /// </summary>
        public async Task<bool> MakeReservationAsync(int customerId, int roomId, DateTime startDate, int duration, ReservationPackage package)
        {
            if (!await CheckAvailabilityAsync(roomId, startDate, duration))
                return false;

            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                return false;

            decimal basePrice = (await _roomRepository.GetByIdAsync(roomId))?.PricePerNight ?? 0;
            decimal finalPrice = await _discountService.CalculateDiscountAsync(customerId, DateTime.UtcNow, startDate, basePrice, package);

            var reservation = new Reservation
            {
                CustomerId = customerId,
                RoomId = roomId,
                StartDate = startDate,
                EndDate = startDate.AddDays(duration),
                Package = package,
                ReservationStatus = ReservationStatus.Confirmed,
                TotalPrice = finalPrice
            };

            await _reservationRepository.AddAsync(reservation);

            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room != null)
            {
                room.RoomStatus = RoomStatus.Occupied;
                await _roomRepository.UpdateAsync(room);
            }

            return true;
        }

        public async Task<int> CreateAndReturnIdAsync(int customerId, int roomId, DateTime checkIn, int duration, ReservationPackage package, decimal totalPrice)
        {
            Reservation reservation = new Reservation
            {
                CustomerId = customerId,
                RoomId = roomId,
                StartDate = checkIn,
                EndDate = checkIn.AddDays(duration),
                Package = package,
                ReservationStatus = ReservationStatus.Waiting, // ✅ BURASI ÇOK ÖNEMLİ
                TotalPrice = totalPrice,
                CurrencyCode = "TRY",
                CreatedDate = DateTime.Now
            };

            await _reservationRepository.CreateAsync(reservation);
            return reservation.Id;
        }

        public async Task<ReservationDto> GetByIdWithRoomAsync(int reservationId)
        {
            Reservation entity = await _reservationRepository
         .GetFirstOrDefaultAsync(
             x => x.Id == reservationId,
             include => include.Include(x => x.Room)
         );

            return _mapper.Map<ReservationDto>(entity);
        }

        public async Task<bool> UpdateAfterPaymentAsync(int reservationId, int paymentId)
        {
            Reservation reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null) return false;

            reservation.ReservationStatus = ReservationStatus.Confirmed;
            reservation.ModifiedDate = DateTime.Now;

            await _reservationRepository.UpdateAsync(reservation);
            return true;
        }

        public async Task<List<ReservationDto>> GetAllWithRoomAndCustomerAsync()
        {
           List<Reservation> reservations = await _reservationRepository.GetAllWithIncludeAsync(
           predicate: x => true,
           include: query => query
          .Include(x => x.Room)
          .Include(x => x.Customer)
           );

            return _mapper.Map<List<ReservationDto>>(reservations);
        }

        public async Task<ReservationDto> GetByIdWithRoomAndCustomerAsync(int reservationId)
        {
            Reservation reservation = await _reservationRepository.GetFirstOrDefaultAsync(
                predicate: x => x.Id == reservationId,
                include: query => query
                    .Include(x => x.Room)
                    .Include(x => x.Customer)
            );

            return _mapper.Map<ReservationDto>(reservation);
        }

        public async Task<bool> UpdateStatusAsync(int reservationId, ReservationStatus newStatus)
        {
            Reservation reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null)
                return false;

            if (reservation.ReservationStatus == newStatus)
                return true;

            reservation.ReservationStatus = newStatus;
            reservation.ModifiedDate = DateTime.Now;

            await _reservationRepository.UpdateAsync(reservation); // sadece await
            return true;
        }

        public async Task<List<Reservation>> GetAllWithIncludeAsync(Expression<Func<Reservation, bool>> predicate, Func<IQueryable<Reservation>, IQueryable<Reservation>> include)
        {
            return await _reservationRepository.GetAllWithIncludeAsync(predicate, include);
        }

        public async Task<int> AddAsync(ReservationDto reservationDto)
        {
            Reservation reservation = _mapper.Map<Reservation>(reservationDto);
            await _reservationRepository.AddAsync(reservation);
            return reservation.Id;
        }

        public  async Task<List<ReservationDto>> GetTodayCheckOutsAsync()
        {
            DateTime today = DateTime.Today;

            IEnumerable<Reservation> reservations = await _reservationRepository.GetAllWithIncludeAsync(
                r => r.ReservationStatus == ReservationStatus.Confirmed && r.EndDate.Date == today,
                q => q
                    .Include(r => r.Customer)
                        .ThenInclude(c => c.User)
                    .Include(r => r.Room)
            );

            return _mapper.Map<List<ReservationDto>>(reservations);
        }

        public async Task<List<ReservationDto>> GetWithIncludeAsync()
        {
             List<Reservation> reservations = await _reservationRepository
            .GetAllWithIncludeAsync(x => true, q => q
            .Include(x => x.Customer)
            .Include(x => x.Room));

            return _mapper.Map<List<ReservationDto>>(reservations);
        }

       

        public async Task<bool> CompleteReservationAsync(int reservationId)
        {
            Reservation? reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null) return false;

            reservation.ReservationStatus = ReservationStatus.Completed;
            reservation.ModifiedDate = DateTime.Now;

            await _reservationRepository.UpdateAsync(reservation);
            return true;
        }

        public  async Task<ReservationDto> GetWithIncludeAsync(int reservationId)
        {
            // Reservation tablosuna, ilişkili Room ve Customer'ı include ederek getiriyoruz
            Reservation entity = await _reservationRepository
                .GetFirstOrDefaultAsync(
                    predicate: r => r.Id == reservationId,
                    include: q => q
                        .Include(r => r.Room)
                        .Include(r => r.Customer)
                );

            if (entity == null)
                return null;

            return _mapper.Map<ReservationDto>(entity);
        }

        public async Task<int> CreateAndReturnIdAsync(int customerId, int userId, int roomId, DateTime checkIn, int duration, ReservationPackage package, decimal totalPrice)
        {
            Console.WriteLine($"[LOG] EF'e yazılacak TotalPrice: {totalPrice}");
            Reservation reservation = new Reservation
            {
                CustomerId = customerId,
                UserId = userId, // ✅ Fark bu!
                RoomId = roomId,
                StartDate = checkIn,
                EndDate = checkIn.AddDays(duration),
                Package = package,
                TotalPrice = totalPrice,
                ReservationStatus = ReservationStatus.Waiting,
                CreatedDate = DateTime.Now
            };

            await _reservationRepository.AddAsync(reservation);
            Console.WriteLine($"[LOG] Reservation.Id: {reservation.Id} | TotalPrice (EF'den sonra): {reservation.TotalPrice}");
            return reservation.Id;
        }
    }
}
