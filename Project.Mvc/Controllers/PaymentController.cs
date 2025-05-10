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
using AutoMapper;
using Project.BLL.Services.abstracts;
using Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Reservation;
using System;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Identity;
using Project.Common.Tools;

namespace Project.MvcUI.Controllers
{
    /*"PaymentController, rezervasyon sonrası ödeme işlemlerini yöneten yapıdır.
Kullanıcının ödeme formundan gelen bilgiler doğrulanır, rezervasyon verisi TempData üzerinden taşınarak
hem Reservation hem Payment kayıtları oluşturulur.
Ödeme API entegrasyonu ile birlikte güvenli ve modüler bir ödeme süreci kurgulanmıştır.
Mapping işlemleriyle form verisi DTO’ya dönüştürülüp, veri tabanına geçmeden önce kontrol ve zenginleştirme sağlanır."*/
    [Route("Payment")]
    public class PaymentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IReservationManager _reservationManager;
        private readonly ICampaignManager _campaignManager;
        private readonly IPaymentManager _paymentManager;
        private readonly ICustomerManager _customerManager;
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public PaymentController(
            IHttpClientFactory httpClientFactory,
            IReservationManager reservationManager,
            ICustomerManager customerManager,IMapper mapper,IPaymentManager paymentManager,ICampaignManager campaignManager,IUserManager userManager)
        {
            _httpClientFactory = httpClientFactory;
            _reservationManager = reservationManager;
            _customerManager = customerManager;
            _mapper = mapper;
            _userManager = userManager;
          _campaignManager = campaignManager;
            _paymentManager = paymentManager;
        }

        /// <summary>
        /// Rezervasyona ait ödeme ekranını getirir.
        /// </summary>
        /// <param name="reservationId">Ödeme yapılacak rezervasyonun ID'si</param>
        /// <returns>Ödeme sayfası</returns>
        [HttpGet("Pay")]
        public async Task<IActionResult> Pay()
        {
            if (TempData["TempReservation"] is not string json)
                return RedirectToAction("Index", "Home");

            ReservationTempModel tempModel = JsonConvert.DeserializeObject<ReservationTempModel>(json);

            ReservationPaymentPageVm vm = new ReservationPaymentPageVm
            {
                TotalPrice = tempModel.TotalPrice,
                PaymentRequest = new PaymentRequestModel
                {
                    ShoppingPrice = tempModel.TotalPrice,
                    Currency = "TRY"
                }
            };

            TempData.Keep("TempReservation");
            return View(vm);
        }
        /// <summary>
        /// Kullanıcıdan gelen ödeme formu işlenir ve ödeme kaydı yapılır.
        /// </summary>
        [HttpPost("Pay")]
        public async Task<IActionResult> Pay(ReservationPaymentPageVm vm)
        {
            // 🔒 Kart bilgisi kontrolü
            if (string.IsNullOrEmpty(vm.PaymentRequest.CardNumber) ||
                string.IsNullOrEmpty(vm.PaymentRequest.CardUserName) ||
                vm.PaymentRequest.CVV.Length < 3)
            {
                ModelState.AddModelError("", "Lütfen geçerli kart bilgilerini giriniz.");
                return View(vm);
            }

            // 🧾 TempReservation'dan verileri al
            if (TempData["TempReservation"] is not string json)
            {
                ModelState.AddModelError("", "Rezervasyon bilgisi bulunamadı.");
                return View(vm);
            }

            TempData.Keep("TempReservation"); // Bir sonraki post için sakla
            ReservationTempModel tempModel = JsonConvert.DeserializeObject<ReservationTempModel>(json);
            int? campaignId = await _campaignManager.MatchCampaignAsync(tempModel.CheckIn, tempModel.Package);


            // 🧑 Giriş yapan kullanıcı bilgisi
            string? userIdStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            int userId = string.IsNullOrEmpty(userIdStr) ? 0 : int.Parse(userIdStr);

            CustomerDto? customer = await _customerManager.GetByUserIdAsync(userId);
            if (customer == null)
            {
                ModelState.AddModelError("", "Kullanıcıya ait müşteri kaydı bulunamadı.");
                return View(vm);
            }

            // 💳 Kart bilgisi maskele
            string maskedCard = vm.PaymentRequest.CardNumber.Length >= 4
                ? "**** **** **** " + vm.PaymentRequest.CardNumber[^4..]
                : vm.PaymentRequest.CardNumber;


            decimal exchangeRate = 1; // Sabit
            decimal convertedAmount = tempModel.TotalPrice; // Direkt TL

            // ✅ Ödeme başarılı → şimdi EF’ye yaz
            int reservationId = await _reservationManager.CreateAndReturnIdAsync(
            tempModel.CustomerId,
            userId,
            tempModel.RoomId,
            tempModel.CheckIn,
            tempModel.Duration,
            tempModel.Package,
            tempModel.TotalPrice,
            tempModel.NumberOfGuests,
            campaignId,
            tempModel.DiscountRate,
             "TRY"
           );

            // 💳 Ödeme DTO’su
            // ✅ Mapping işlemiyle formdan gelen ödeme bilgileri PaymentDto'ya çevrilir
            // 🎯 Böylece formdan gelen verilerle birlikte sistem içinde hesaplanan alanlar da eklenmiş olur
            //“Kredi kartı bilgileri ve kullanıcıdan gelen form verileri PaymentDto'ya AutoMapper ile dönüştürülüyor,
            // ardından sistem tarafından eklenen tarih, fiyat, kullanıcı ID gibi verilerle zenginleştirilerek API'ye gönderiliyor.”
            PaymentDto dto = _mapper.Map<PaymentDto>(vm.PaymentRequest);
            dto.ReservationId = reservationId;
            dto.UserId = userId;
            dto.CustomerId = customer.Id;
            dto.TotalAmount = tempModel.TotalPrice;
            dto.PaidAmount = tempModel.TotalPrice;
            dto.PaymentStatus = PaymentStatus.Completed;
            dto.PaymentMethod = PaymentMethod.CreditCard;
            dto.CreatedDate = DateTime.Now;
            dto.PaymentDate = DateTime.Now;
            dto.LastUpdated = DateTime.Now;
            dto.InvoiceNumber = $"INV-{DateTime.Now.Ticks}";
            dto.Description = $"Kart Sahibi: {vm.PaymentRequest.CardUserName}, Kart: {maskedCard}";
            dto.ExchangeRate = exchangeRate;
            dto.IsAdvancePayment = false;
            dto.IsRefunded = false;
            dto.TransactionId = Guid.NewGuid().ToString();
            dto.CancellationReason = "";

            // 🌐 Ödeme API'sine istek gönderilir
            try
            {

                HttpClient client = _httpClientFactory.CreateClient();
                string paymentJson = JsonConvert.SerializeObject(dto);
                HttpContent content = new StringContent(paymentJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("http://localhost:5126/api/PaymentApi/create", content);

                if (!response.IsSuccessStatusCode)
                {
                    string error = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", "Ödeme başarısız: " + error);
                    return View(vm);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmeyen bir hata oluştu: " + ex.Message);
                return View(vm);
            }
            // ✅ Kullanıcının e-postasını bul
            UserDto user = await _userManager.GetByIdAsync(userId);
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                string emailBody = $@"
               <h3>Sayın Müşterimiz,</h3>
               <p>Aşağıdaki rezervasyon bilgilerinizle birlikte ödemeniz başarıyla alınmıştır:</p>
              <table style='font-size:14px;'>
              <tr><td><strong>Oda Tipi:</strong></td><td>{tempModel.RoomType}</td></tr>
              <tr><td><strong>Giriş Tarihi:</strong></td><td>{tempModel.CheckIn:dd.MM.yyyy}</td></tr>
              <tr><td><strong>Çıkış Tarihi:</strong></td><td>{tempModel.CheckOut:dd.MM.yyyy}</td></tr>
              <tr><td><strong>Kişi Sayısı:</strong></td><td>{tempModel.NumberOfGuests}</td></tr>
              <tr><td><strong>Toplam Tutar:</strong></td><td>{tempModel.TotalPrice:N2} ₺</td></tr>
              <tr><td><strong>Fatura No:</strong></td><td>{dto.InvoiceNumber}</td></tr>
              <tr><td><strong>Ödeme Tarihi:</strong></td><td>{dto.PaymentDate:dd.MM.yyyy}</td></tr>
              </table>
              <p>Teşekkür ederiz.<br/><strong>BilgeHotel Ekibi</strong></p>";

                bool mailSent = EmailService.Send(user.Email, emailBody, "BilgeHotel - Ödeme ve Rezervasyon Bilgisi");
                if (mailSent)
                    Console.WriteLine("📩 Fatura e-postası başarıyla gönderildi.");
                else
                    Console.WriteLine("❌ Fatura e-postası gönderilemedi.");
            }

            return RedirectToAction("Success", "Payment");
        }

        /// <summary>
        /// Başarılı ödeme sonrası gösterilen ekran
        /// </summary>
        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}

