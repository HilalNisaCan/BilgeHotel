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



        public async Task<CustomerDto> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> GetOrCreateCustomerAsync(int userId, string identityNumber, string firstName, string lastName, int birthYear)
        {
            Customer? existing = await _customerRepository.GetFirstOrDefaultAsync(x => x.UserId == userId, null);


            if (existing != null)
                return _mapper.Map<CustomerDto>(existing);

            var profile = await _userProfileRepository.GetByUserIdAsync(userId);

            if (profile == null)
                throw new Exception("Profil bulunamadı.");

            var customer = new Customer
            {
                UserId = userId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                IdentityNumber = identityNumber,
                PhoneNumber = profile.PhoneNumber, // ❗ Hatanın kaynağı buydu
                BillingDetails = "Online Ödeme Fatura Bilgisi" // NULL hatası almamak için sabit verilebilir

            };

            await _customerRepository.AddAsync(customer);

            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<List<ReservationDto>> GetCustomerReservationsAsync(int customerId)
        {
            var reservations = await _reservationRepository.GetAllAsync(r => r.CustomerId == customerId);
            return _mapper.Map<List<ReservationDto>>(reservations);
        }

        public async Task<decimal> GetLoyaltyPointsAsync(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            return customer?.LoyaltyPoints ?? 0;
        }

        public async Task UpdateLoyaltyPointsAsync(int customerId, decimal points)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer != null)
            {
                customer.LoyaltyPoints = (int)points;
                await _customerRepository.UpdateAsync(customer);
            }
        }
        /// <summary>
        /// Müşterinin kimlik bilgilerini doğrular (Mernis veya Mock servis).
        /// </summary>
        public async Task<bool> VerifyCustomerIdentityAsync(KimlikBilgisiDto dto)
        {
            return await _identityValidationService.VerifyAsync(dto);
        }

        public async Task<CustomerDto?> GetByUserIdAsync(int userId)
        {
            Customer? customer = await _customerRepository.GetFirstOrDefaultAsync(
            c => c.UserId == userId,
            include: null
            );


            return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
        }

       
       

        public async Task<int> AddAsync(CustomerDto dto)
        {
            Customer entity = _mapper.Map<Customer>(dto);
            return await _customerRepository.AddAndReturnIdAsync(entity);
        }

        public async Task<List<CustomerReportDto>> GetAllCustomerReportsAsync()
        {
            var customers = await _customerRepository.GetAllWithIncludeAsync(null, q =>
        q.Include(x => x.Reservations)
         .Include(x => x.Payments));

            return customers.Select(c => new CustomerReportDto
            {
                Id = c.Id,
                FullName = $"{c.FirstName} {c.LastName}",
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

          
        }

    }
}
