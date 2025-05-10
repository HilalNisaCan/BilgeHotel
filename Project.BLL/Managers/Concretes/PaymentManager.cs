using AutoMapper;
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
    /// <summary>
    /// Ödeme işlemlerini yöneten manager sınıfı.
    /// </summary>
    public class PaymentManager : BaseManager<PaymentDto, Payment>, IPaymentManager
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentManager(
            IPaymentRepository paymentRepository,
            IMapper mapper)
            : base(paymentRepository, mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni ödeme oluşturur (ID dönmez).
        /// </summary>
        public async Task<bool> AddAsync(PaymentDto dto)
        {
            Payment entity = _mapper.Map<Payment>(dto);
            await _paymentRepository.AddAsync(entity);
            return true;
        }

        /// <summary>
        /// Yeni ödeme oluşturur ve ID’yi döner.
        /// </summary>
        public async Task<int> CreateAndReturnIdAsync(PaymentDto dto)
        {
            Payment entity = _mapper.Map<Payment>(dto);
            await _paymentRepository.CreateAsync(entity);
            return entity.Id;
        }
    }
}