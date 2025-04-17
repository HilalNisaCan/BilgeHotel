using Microsoft.AspNetCore.Mvc;
using Project.BLL.DtoClasses;
using Project.BLL.Managers.Abstracts;

namespace Project.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentApiController : ControllerBase
    {
        private readonly IPaymentManager _paymentManager;

        public PaymentApiController(IPaymentManager paymentManager)
        {
            _paymentManager = paymentManager;
        }

        // 🔹 Belirli bir rezervasyona ait ödeme bilgisi
        [HttpGet("reservation/{reservationId}")]
        public async Task<IActionResult> GetByReservationId(int reservationId)
        {
            var payment = await _paymentManager.GetByIdAsync(reservationId);

            if (payment == null)
                return NotFound("Ödeme bilgisi bulunamadı.");

            return Ok(new
            {
                payment.Id,
                payment.TotalAmount,
                payment.PaidAmount,
                payment.PaymentStatus,
                payment.PaymentMethod,
                payment.CreatedDate,
                payment.ReservationId,
                payment.InvoiceNumber
            });
        }

        // 🔹 Ödeme kaydı oluşturma
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto dto)
        {
            Console.WriteLine("🔥 API'ye veri geldi mi?");
            Console.WriteLine("🎯 ReservationId: " + dto.ReservationId);
            Console.WriteLine("🎯 UserId: " + dto.UserId);
            Console.WriteLine("🎯 TotalAmount: " + dto.TotalAmount);

            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState Invalid");
                return BadRequest(ModelState);
            }

            int result = await _paymentManager.CreateAndReturnIdAsync(dto);

            if (result <= 0)
            {
                Console.WriteLine("❌ Payment kaydedilemedi.");
                return BadRequest("Ödeme kaydedilemedi.");
            }

            Console.WriteLine("✅ PaymentId: " + result);
            return Ok(new { PaymentId = result });
        }
    }
}
