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
        private new readonly IMapper _mapper;

       
        public ReservationManager(IReservationRepository reservationRepo, IMapper mapper, IEarlyReservationDiscountManager discountService) : base(reservationRepo, mapper)
        {
            _reservationRepository = reservationRepo;
            _mapper = mapper;
           
        }


        /// <summary>
        /// Belirtilen müşterinin geçmiş rezervasyonlarını DTO olarak getirir.
        /// Kullanıldığı yer: Kullanıcı paneli → "Rezervasyonlarım" ekranı.
        /// </summary>
        public async Task<List<ReservationDto>> GetCustomerReservationsAsync(int customerId)
        {
            List<Reservation> reservations = (await _reservationRepository.GetAllAsync(r => r.CustomerId == customerId)).ToList();
            return _mapper.Map<List<ReservationDto>>(reservations.OrderByDescending(r => r.StartDate).ToList());
        }



        /// <summary>
        /// Yeni rezervasyon oluşturur ve ID’yi döner (UserId olmayan senaryo).
        /// Kullanıldığı yer: Kayıtsız kullanıcı (sadece CustomerId) ile rezervasyon.
        /// </summary>
        public async Task<int> CreateAndReturnIdAsync(int customerId, int roomId, DateTime checkIn, int duration, ReservationPackage package, decimal totalPrice)
        {
            Reservation reservation = new Reservation
            {
                CustomerId = customerId,
                RoomId = roomId,
                StartDate = checkIn.Date.AddHours(14), // Giriş: 14:00
                EndDate = checkIn.Date.AddDays(duration).AddHours(10), // Çıkış: 10:00
                Package = package,
                ReservationStatus = ReservationStatus.Waiting,
                TotalPrice = totalPrice,
                CurrencyCode = "TRY",
                CreatedDate = DateTime.Now,
                CheckInTime = new TimeSpan(14, 0, 0),
                ReservationDate = DateTime.Now
            };

            await _reservationRepository.CreateAsync(reservation);
            return reservation.Id;
        }



        /// <summary>
        /// Oda ve müşteri bilgileri dahil tüm rezervasyonları getirir.
        /// Kullanıldığı yer: Admin paneli → Rezervasyon yönetimi ekranı.
        /// </summary>
        public async Task<List<ReservationDto>> GetAllWithRoomAndCustomerAsync()
        {
            List<Reservation> reservations = await _reservationRepository.GetAllWithIncludeAsync(
                x => true,
                query => query.Include(x => x.Room).Include(x => x.Customer)
            );

            return _mapper.Map<List<ReservationDto>>(reservations);
        }

        /// <summary>
        /// Belirli rezervasyonu oda ve müşteri bilgileriyle birlikte getirir.
        /// </summary>
        public async Task<ReservationDto> GetByIdWithRoomAndCustomerAsync(int reservationId)
        {
            Reservation reservation = await _reservationRepository.GetFirstOrDefaultAsync(
                x => x.Id == reservationId,
                query => query.Include(x => x.Room).Include(x => x.Customer)
            );

            return _mapper.Map<ReservationDto>(reservation);
        }

        /// <summary>
        /// Rezervasyonun durumunu (Onaylandı, İptal, Bekliyor) günceller.
        /// </summary>
        public async Task<bool> UpdateStatusAsync(int reservationId, ReservationStatus newStatus)
        {
            Reservation reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null) return false;

            if (reservation.ReservationStatus == newStatus) return true;

            reservation.ReservationStatus = newStatus;
            reservation.ModifiedDate = DateTime.Now;

            await _reservationRepository.UpdateAsync(reservation);
            return true;
        }

        /// <summary>
        /// Özel filtre ve include ile rezervasyonları getirir.
        /// </summary>
        public async Task<List<Reservation>> GetAllWithIncludeAsync(
            Expression<Func<Reservation, bool>> predicate,
            Func<IQueryable<Reservation>, IQueryable<Reservation>> include)
        {
            return await _reservationRepository.GetAllWithIncludeAsync(predicate, include);
        }

        /// <summary>
        /// DTO üzerinden rezervasyon oluşturur ve ID’yi döner.
        /// </summary>
        public async Task<int> AddAsync(ReservationDto reservationDto)
        {
            Reservation reservation = _mapper.Map<Reservation>(reservationDto);
            await _reservationRepository.AddAsync(reservation);
            return reservation.Id;
        }



        /// <summary>
        /// Tüm rezervasyonları oda ve müşteri ile birlikte DTO olarak getirir.
        /// </summary>
        public async Task<List<ReservationDto>> GetWithIncludeAsync()
        {
            List<Reservation> reservations = await _reservationRepository.GetAllWithIncludeAsync(
                x => true,
                q => q.Include(x => x.Customer).Include(x => x.Room)
            );

            return _mapper.Map<List<ReservationDto>>(reservations);
        }



        /// <summary>
        /// Rezervasyonun durumunu “Tamamlandı” olarak işaretler.
        /// </summary>
        public async Task<bool> CompleteReservationAsync(int reservationId)
        {
            Reservation? reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null) return false;

            reservation.ReservationStatus = ReservationStatus.Completed;
            reservation.ModifiedDate = DateTime.Now;

            reservation.EndDate = reservation.EndDate.Date.AddHours(10); // Checkout: 10:00

            await _reservationRepository.UpdateAsync(reservation);
            return true;
        }

        /// <summary>
        /// Belirli rezervasyonu, oda ve müşteri bilgileriyle birlikte getirir (tekil DTO).
        /// </summary>
        public async Task<ReservationDto> GetWithIncludeAsync(int reservationId)
        {
            Reservation entity = await _reservationRepository.GetFirstOrDefaultAsync(
                r => r.Id == reservationId,
                q => q.Include(r => r.Room).Include(r => r.Customer)
            );

            if (entity == null) return null;

            return _mapper.Map<ReservationDto>(entity);
        }

        /// <summary>
        /// Yeni rezervasyon oluşturur ve ID’yi döner (UserId içeren versiyon).
        /// Kullanıldığı yer: Sisteme giriş yapmış kullanıcı ile rezervasyon.
        /// </summary>
        public async Task<int> CreateAndReturnIdAsync(
        int customerId,
        int userId,
        int roomId,
        DateTime checkIn,
        int duration,
        ReservationPackage package,
        decimal totalPrice,
        int numberOfGuests,
        int? campaignId = null,
        decimal discountRate = 0,
        string currencyCode = "TRY")
        {
            Reservation reservation = new Reservation
            {
                CustomerId = customerId,
                UserId = userId,
                RoomId = roomId,
                StartDate = checkIn.Date.AddHours(14),
                EndDate = checkIn.Date.AddDays(duration).AddHours(10),
                Package = package,
                ReservationStatus = ReservationStatus.Waiting,
                TotalPrice = totalPrice,
                DiscountRate = discountRate,
                NumberOfGuests = numberOfGuests,
                CampaignId = campaignId,
                CurrencyCode = string.IsNullOrEmpty(currencyCode) ? "TRY" : currencyCode,
                CreatedDate = DateTime.Now,
                ReservationDate = DateTime.Now,
                CheckInTime = new TimeSpan(14, 0, 0)
            };

            await _reservationRepository.AddAsync(reservation);
            return reservation.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null) return false;

            if (reservation.ReservationStatus == ReservationStatus.Confirmed)
                throw new Exception("Onaylı rezervasyon silinemez.");

            return await _reservationRepository.DeleteReservationAsync(id);
        }
    }
}
