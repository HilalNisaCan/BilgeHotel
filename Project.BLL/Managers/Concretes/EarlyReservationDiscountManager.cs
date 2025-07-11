﻿using AutoMapper;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Managers.Concretes
{
    /// <summary>
    /// Erken rezervasyon indirimlerini hesaplayan manager sınıfı.
    /// </summary>
    public class EarlyReservationDiscountManager : BaseManager<EarlyReservationDiscountDto, EarlyReservationDiscount>, IEarlyReservationDiscountManager
    {
        private readonly IEarlyReservationDiscountRepository _discountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICampaignRepository _campaignRepository;    
        private readonly IMapper _mapper;

        public EarlyReservationDiscountManager(
            IEarlyReservationDiscountRepository discountRepository,
            ICustomerRepository customerRepository,
            ICampaignRepository campaignRepository,     
            IMapper mapper)
            : base(discountRepository, mapper)
        {
            _discountRepository = discountRepository;
            _customerRepository = customerRepository;
            _campaignRepository = campaignRepository;
            _mapper = mapper;
        }


        /// <summary>
        /// Erken rezervasyon, sadakat ve kampanyaları dikkate alarak toplam indirim oranını hesaplar.
        /// </summary>
        public async Task<decimal> CalculateDiscountAsync(int customerId, DateTime reservationDate, DateTime checkInDate, decimal basePrice, ReservationPackage package)
        {
            int daysBeforeCheckIn = (checkInDate - reservationDate).Days;
            decimal discountRate = 0m;

            // Erken rezervasyon indirimi
            if (daysBeforeCheckIn >= 90)
                discountRate = Math.Max(discountRate, 0.23m);
            else if (daysBeforeCheckIn >= 30 && package == ReservationPackage.AllInclusive)
                discountRate = Math.Max(discountRate, 0.18m);
            else if (daysBeforeCheckIn >= 30 && package == ReservationPackage.Fullboard)
                discountRate = Math.Max(discountRate, 0.16m);

            // Sadakat indirimi
            Customer? customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer != null && customer.LoyaltyPoints >= 100)
                discountRate += 0.05m;

            // Aktif kampanya indirimi
            Campaign? activeCampaign = (await _campaignRepository.GetAllAsync(c => c.Package == package && c.IsActive))
                .OrderByDescending(c => c.DiscountPercentage)
                .FirstOrDefault();

            if (activeCampaign != null)
            {
                decimal campaignRate = activeCampaign.DiscountPercentage / 100m;
                discountRate += campaignRate;
            }

            // İndirim yüzdesi olarak döndürülür
            return Math.Round(discountRate * 100, 2);
        }
    }
}
