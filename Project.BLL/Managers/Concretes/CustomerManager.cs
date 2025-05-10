using AutoMapper;
using Castle.Core.Resource;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Services;
using Project.BLL.Services.abstracts;
using Project.BLL.Services.Concretes;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Entities.Enums;

namespace Project.BLL.Managers.Concretes
{
    public class CustomerManager : BaseManager<CustomerDto, Customer>, ICustomerManager
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IIdentityValidationService _identityValidationService;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;

        public CustomerManager(
            ICustomerRepository customerRepository,
            IReservationRepository reservationRepository,
            IUserRepository userRepository,IIdentityValidationService
            identityValidationService,
          
       
            IMapper mapper,IUserProfileRepository userProfileRepository
        ) : base(customerRepository, mapper)
        {
            _customerRepository = customerRepository;
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
           _identityValidationService = identityValidationService;
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }



        /// <summary>
        /// Kullanıcının müşteri kaydı yoksa oluşturur, varsa mevcut kaydı döner.
        /// </summary>
        public async Task<CustomerDto> GetOrCreateCustomerAsync(int userId, string identityNumber, string firstName, string lastName, int birthYear)
        {
            Customer? existing = await _customerRepository.GetFirstOrDefaultAsync(x => x.UserId == userId, null);
            if (existing != null)
                return _mapper.Map<CustomerDto>(existing);

            UserProfile? profile = await _userProfileRepository.GetByUserIdAsync(userId);
            if (profile == null)
                throw new Exception("Profil bulunamadı.");

            Customer customer = new Customer
            {
                UserId = userId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                IdentityNumber = identityNumber,
                PhoneNumber = profile.PhoneNumber,
                BillingDetails = "Online Ödeme Fatura Bilgisi"
            };

            await _customerRepository.AddAsync(customer);
            return _mapper.Map<CustomerDto>(customer);
        }

        /// <summary>
        /// Müşterinin tüm rezervasyonlarını getirir.
        /// </summary>
        public async Task<List<ReservationDto>> GetCustomerReservationsAsync(int customerId)
        {
            List<Reservation> reservations = (await _reservationRepository.GetAllAsync(r => r.CustomerId == customerId)).ToList();
            return _mapper.Map<List<ReservationDto>>(reservations);
        }




        /// <summary>
        /// Müşterinin kimlik bilgilerini doğrular (TCKN, ad, soyad, doğum yılı).
        /// </summary>
        public async Task<bool> VerifyCustomerIdentityAsync(KimlikBilgisiDto dto)
        {
            return await _identityValidationService.VerifyAsync(dto);
        }

        /// <summary>
        /// UserId üzerinden müşteri kaydını getirir.
        /// </summary>
        public async Task<CustomerDto?> GetByUserIdAsync(int userId)
        {
            Customer? customer = await _customerRepository.GetFirstOrDefaultAsync(
                c => c.UserId == userId,
                include: null
            );

            return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
        }


        /// <summary>
        /// Yeni müşteri oluşturur ve ID’yi döner.
        /// </summary>
        public async Task<int> AddAsync(CustomerDto dto)
        {
            Customer entity = _mapper.Map<Customer>(dto);
            return await _customerRepository.AddAndReturnIdAsync(entity);
        }

        /// <summary>
        /// Tüm müşterilerin rapor verilerini listeler (rezervasyon, harcama, puan vs.).
        /// </summary>
        public async Task<List<CustomerReportDto>> GetAllCustomerReportsAsync()
        {
            List<Customer> customers = (await _customerRepository.GetAllWithIncludeAsync(
                null,
                q => q.Include(x => x.Reservations)
                      .Include(x => x.Payments)
            )).ToList();

            List<CustomerReportDto> reportList = customers.Select(c => new CustomerReportDto
            {
                Id = c.Id,
                FullName = $"{c.FirstName} {c.LastName.First()}***",
                IdentityNumber = c.IdentityNumber,
                PhoneNumber = c.PhoneNumber,
                LoyaltyPoints = c.LoyaltyPoints,

                TotalReservationCount = c.Reservations?.Count ?? 0,
                LastReservationDate = c.Reservations?
                    .OrderByDescending(r => r.EndDate)
                    .FirstOrDefault()?.EndDate,
                TotalSpent = c.Payments?.Sum(p => p.TotalAmount) ?? 0,
                CampaignUsageCount = c.Reservations?.Count(r => r.CampaignId != null) ?? 0,

                ReservationCount = c.Reservations?.Count ?? 0,
                CampaignCount = c.Reservations?.Count(r => r.CampaignId != null) ?? 0
            }).ToList();

            return reportList;
        }

        public async Task<CustomerReportDto?> GetCustomerReportWithReservationsAsync(int id)
        {

            // 1. Customer + User + UserProfile + Reservations verilerini getir
            Customer? customer = await _customerRepository.GetByIdWithUserAndReservationsAsync(id);

            // 2. Boş kontrolü yap
            if (customer == null)
                return null;

            string fullName = customer.User?.UserProfile != null
       ? $"{customer.User.UserProfile.FirstName} {customer.User.UserProfile.LastName.First()}***"
       : $"{customer.FirstName} {customer.LastName.First()}***";

            // 3. Rezervasyon boşsa sıfırla
            List<Reservation> reservations = customer.Reservations?.ToList() ?? new List<Reservation>();

            // 4. Geçmiş rezervasyonları filtrele
            List<ReservationDto> pastReservations = reservations
                .Where(r =>
        r.EndDate < DateTime.Today ||
        r.ReservationStatus == ReservationStatus.Cancelled ||
        r.ReservationStatus == ReservationStatus.Completed)
                .Select(r => new ReservationDto
                {
                    Id = r.Id,
                    RoomId = r.RoomId,
                    Package = r.Package,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    TotalPrice = r.TotalPrice,
                    ReservationStatus = r.ReservationStatus,
                    DiscountRate = r.DiscountRate,
                    Room = r.Room != null
                        ? new RoomDto
                        {
                            Id = r.Room.Id,
                            RoomType = r.Room.RoomType
                        }
                        : null
                })
                .ToList();
            // GELECEK rezervasyonları filtrele
            List<ReservationDto> upcomingReservations = reservations
     .Where(r => r.StartDate.Date > DateTime.Today)
     .Select(r => new ReservationDto
     {
         Id = r.Id,
         RoomId = r.RoomId,
         Package = r.Package,
         StartDate = r.StartDate,
         EndDate = r.EndDate,
         TotalPrice = r.TotalPrice,
         ReservationStatus = r.ReservationStatus,
         DiscountRate = r.DiscountRate,
         Room = r.Room != null ? new RoomDto
         {
             Id = r.Room.Id,
             RoomType = r.Room.RoomType,
         } : null
     }).ToList();
            List<ReservationDto> currentStays = reservations
      .Where(r =>
          r.StartDate.Date <= DateTime.Today &&
          r.EndDate.Date >= DateTime.Today &&
          r.ReservationStatus == ReservationStatus.Confirmed)
      .Select(r => new ReservationDto
      {
          Id = r.Id,
          RoomId = r.RoomId,
          Package = r.Package,
          StartDate = r.StartDate,
          EndDate = r.EndDate,
          TotalPrice = r.TotalPrice,
          ReservationStatus = r.ReservationStatus,
          DiscountRate = r.DiscountRate,
          Room = r.Room != null ? new RoomDto
          {
              Id = r.Room.Id,
              RoomType = r.Room.RoomType
          } : null
      })
      .ToList();
            // 5. DTO'yu oluştur ve doldur
            CustomerReportDto dto = new CustomerReportDto
            {
                Id = customer.Id,
                FullName = fullName,
                IdentityNumber = customer.IdentityNumber,
                PhoneNumber = customer.PhoneNumber,
                TotalReservationCount = pastReservations.Count + upcomingReservations.Count + currentStays.Count,
                TotalSpent = reservations.Sum(r => r.TotalPrice),
                LoyaltyPoints = customer.LoyaltyPoints,
                CampaignUsageCount = reservations.Count(r => r.CampaignId != null),
                LastReservationDate = reservations
                    .OrderByDescending(r => r.EndDate)
                    .FirstOrDefault()?.EndDate,
                PastReservations = pastReservations,
                UserId = customer.UserId?.ToString() ?? "-",
                UpcomingReservations = upcomingReservations,
               CurrentStays = currentStays,
            };

            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _customerRepository.SoftDeleteAsync(id);
        }
    }
}
