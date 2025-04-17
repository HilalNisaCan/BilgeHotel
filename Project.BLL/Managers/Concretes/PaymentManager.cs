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
    public class PaymentManager : BaseManager<PaymentDto, Payment>, IPaymentManager
    {

        private readonly IPaymentRepository _paymentRepository;
        private readonly IExtraExpenseRepository _expenseRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public PaymentManager(IPaymentRepository paymentRepository,
                              IExtraExpenseRepository expenseRepository,
                              IOrderDetailRepository orderDetailRepository,
                              IOrderRepository orderRepository,
                              IMapper mapper,
                              IReservationRepository reservationRepository)
            : base(paymentRepository, mapper)
        {
            _paymentRepository = paymentRepository;
            _expenseRepository = expenseRepository;
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _reservationRepository = reservationRepository;
        }

        /// <summary>
        /// Belirtilen rezervasyonun toplam borcunu hesaplar.
        /// </summary>
        public async Task<decimal> CalculateTotalBillAsync(int reservationId)
        {
            var payments = await _paymentRepository.GetAllAsync(p => p.ReservationId == reservationId);
            var extraExpenses = (await _expenseRepository.GetAllAsync(e => e.ReservationId == reservationId)).Sum(e => e.UnitPrice);

            var allOrderDetails = await _orderDetailRepository.GetAllAsync();
            var allOrders = await _orderRepository.GetAllAsync();

            var filteredOrderDetails = allOrderDetails
                .Where(detail =>
                    allOrders.Any(order => order.Id == detail.OrderId && order.ReservationId == reservationId))
                .ToList();

            var orderTotal = filteredOrderDetails.Sum(o => o.UnitPrice);

            return payments.Sum(p => p.TotalAmount) + extraExpenses + orderTotal;
        }

        public async Task<int> CreateAndReturnIdAsync(PaymentDto dto)
        {
            Payment entity = _mapper.Map<Payment>(dto);
            await _paymentRepository.CreateAsync(entity);
            return entity.Id;
        }

        /// <summary>
        /// Belirtilen rezervasyon için yapılan toplam ödemelere göre kalan borcu hesaplar.
        /// </summary>
        public async Task<decimal> GetRemainingBalanceAsync(int reservationId)
        {
            var totalBill = await CalculateTotalBillAsync(reservationId);
            var totalPayments = (await _paymentRepository.GetAllAsync(p =>
                p.ReservationId == reservationId && p.PaymentStatus != PaymentStatus.Cancelled)).Sum(p => p.TotalAmount);

            return totalBill - totalPayments;
        }
        /// <summary>
        /// Rezervasyon için yeni bir ödeme kaydı oluşturur.
        /// Ödeme tamamsa status = Completed, eksikse status = Pending.
        /// </summary>
        public async Task<bool> ProcessPaymentAsync(int reservationId, decimal amount, PaymentMethod method)
        {
            var remainingBalance = await GetRemainingBalanceAsync(reservationId);
            if (amount > remainingBalance)
                throw new Exception("Ödeme miktarı toplam borçtan fazla olamaz!");

            var payment = new Payment
            {
                ReservationId = reservationId,
                TotalAmount = amount,
                PaymentMethod = method,
                PaymentStatus = remainingBalance - amount == 0 ? PaymentStatus.Completed : PaymentStatus.Pending
            };

            await _paymentRepository.AddAsync(payment);
            return true;
        }

        /// <summary>
        /// Belirtilen ödeme kaydı iade edilir. Gerekçe kaydedilir.
        /// </summary>
        public async Task<bool> RefundPaymentAsync(int paymentId, string reason)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null || payment.PaymentStatus == PaymentStatus.Cancelled)
                return false;

            payment.PaymentStatus = PaymentStatus.Refunded;
            payment.CancellationReason = reason;
            payment.LastUpdated = DateTime.UtcNow;

            await _paymentRepository.UpdateAsync(payment);
            return true;
        }

       

        /// <summary>
        /// Manuel olarak ödeme statüsü güncellenir (örneğin Admin tarafından).
        /// </summary>
        public async Task UpdatePaymentStatusAsync(int paymentId, PaymentStatus status)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null) throw new Exception("Ödeme bulunamadı.");

            payment.PaymentStatus = status;
            await _paymentRepository.UpdateAsync(payment);
        }

        
    }
}
