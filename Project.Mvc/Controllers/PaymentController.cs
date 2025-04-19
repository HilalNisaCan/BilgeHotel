using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.Models.PageVm.Reservation;
using Project.MvcUI.PaymentApiTools;
using System.Text;
using System.Security.Claims;

namespace Project.MvcUI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IReservationManager _reservationManager;

        public PaymentController(
            IHttpClientFactory httpClientFactory,
            IReservationManager reservationManager)
        {
            _httpClientFactory = httpClientFactory;
            _reservationManager = reservationManager;
        }

        [HttpGet]
        public async Task<IActionResult> Pay(int reservationId)
        {
            var reservation = await _reservationManager.GetByIdAsync(reservationId);
            if (reservation == null) return NotFound();

            var model = new ReservationPaymentPageVm
            {
                ReservationId = reservation.Id,
                TotalPrice = reservation.TotalPrice,
                PaymentRequest = new PaymentRequestModel
                {
                    ShoppingPrice = reservation.TotalPrice
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Pay(ReservationPaymentPageVm vm)
        {
            var reservation = await _reservationManager.GetByIdAsync(vm.ReservationId);
            if (reservation == null)
            {
                ModelState.AddModelError("", "Rezervasyon bulunamadı.");
                return View(vm);
            }
            Console.WriteLine("💳 [PAYMENT] RezID: " + reservation.Id + " | Fiyat: " + reservation.TotalPrice);

            if (string.IsNullOrEmpty(vm.PaymentRequest.CardNumber) ||
                string.IsNullOrEmpty(vm.PaymentRequest.CardUserName) ||
                vm.PaymentRequest.CVV.Length < 3)
            {
                ModelState.AddModelError("", "Lütfen geçerli kart bilgilerini giriniz.");
                return View(vm);
            }

            string maskedCard = vm.PaymentRequest.CardNumber.Length >= 4
                ? "**** **** **** " + vm.PaymentRequest.CardNumber[^4..]
                : vm.PaymentRequest.CardNumber;

            string? userIdStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            int userId = string.IsNullOrEmpty(userIdStr) ? 0 : int.Parse(userIdStr);

            PaymentDto dto = new PaymentDto
            {
                ReservationId = reservation.Id,
                UserId = userId, // 🔥 Giriş yapan kullanıcıdan alıyoruz
                CustomerId = 1, // Test için dummy (isteğe göre değişir)
                TotalAmount = reservation.TotalPrice,
                PaidAmount = reservation.TotalPrice,
                PaymentStatus = PaymentStatus.Completed,
                PaymentMethod = PaymentMethod.CreditCard,
                CreatedDate = DateTime.Now,
                PaymentDate = DateTime.Now,
                InvoiceNumber = "INV-" + DateTime.Now.Ticks,
                Description = $"Kart Sahibi: {vm.PaymentRequest.CardUserName}, Kart: {maskedCard}",
                ExchangeRate = 1,
                IsAdvancePayment = false,
                IsRefunded = false,
                LastUpdated = DateTime.Now,
                TransactionId = Guid.NewGuid().ToString(),
                CancellationReason = ""
            };

            HttpClient client = _httpClientFactory.CreateClient();
            string json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("http://localhost:5126/api/PaymentApi/create", content);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "Ödeme başarısız: " + error);
                return View(vm);
            }
            TempData["Message"] = "Ödeme başarıyla tamamlandı!";
            return RedirectToAction("Success", "Payment");
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}

