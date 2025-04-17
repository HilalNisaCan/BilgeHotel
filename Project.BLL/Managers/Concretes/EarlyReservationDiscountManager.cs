using AutoMapper;
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
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public EarlyReservationDiscountManager(
            IEarlyReservationDiscountRepository discountRepository,
            ICustomerRepository customerRepository,
            ICampaignRepository campaignRepository,
            IReservationRepository reservationRepository,
            IMapper mapper)
            : base(discountRepository, mapper)
        {
            _discountRepository = discountRepository;
            _customerRepository = customerRepository;
            _campaignRepository = campaignRepository;
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Belirli bir rezervasyona uygun indirimi hesaplar ve toplam fiyata uygular.
        /// </summary>
        public async Task<bool> ApplyDiscountToReservationAsync(int reservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null)
                return false;

            decimal newPrice = await CalculateDiscountAsync(
                reservation.CustomerId,
                reservation.StartDate,
                reservation.StartDate,
                reservation.TotalPrice,
                reservation.Package);

            reservation.TotalPrice = newPrice;
            await _reservationRepository.UpdateAsync(reservation);

            return true;
        }

        /// <summary>
        /// Tüm kriterleri kontrol ederek toplam indirimi hesaplar.
        /// </summary>
        public async Task<decimal> CalculateDiscountAsync(int customerId, DateTime reservationDate, DateTime checkInDate, decimal basePrice, ReservationPackage package)
        {
            int daysBeforeCheckIn = (checkInDate - reservationDate).Days;

            // 🔹 İndirim oranı yüzde cinsinden ondalık olarak hesaplanmalı
            decimal discountRate = 0m;

            // ✅ Erken rezervasyon indirimi
            if (daysBeforeCheckIn >= 90)
                discountRate = Math.Max(discountRate, 0.23m); // %23
            else if (daysBeforeCheckIn >= 30 && package == ReservationPackage.AllInclusive)
                discountRate = Math.Max(discountRate, 0.18m); // %18
            else if (daysBeforeCheckIn >= 30 && package == ReservationPackage.Fullboard)
                discountRate = Math.Max(discountRate, 0.16m); // %16

            // ✅ Sadakat indirimi
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer != null && customer.LoyaltyPoints >= 100)
                discountRate += 0.05m; // %5 ek

            // ✅ Kampanya indirimi (ayrı ayrı değerlendirilir)
            var activeCampaign = (await _campaignRepository.GetAllAsync(c => c.Package == package && c.IsActive))
                                 .OrderByDescending(c => c.DiscountPercentage)
                                 .FirstOrDefault();

            if (activeCampaign != null)
            {
                decimal campaignRate = activeCampaign.DiscountPercentage / 100m;
                discountRate += campaignRate;
            }

            // 🔹 Toplam indirimli fiyat
            decimal discountAmount = basePrice * discountRate;
            decimal finalPrice = basePrice - discountAmount;

            return Math.Round(finalPrice, 2);
        }
    }
}
