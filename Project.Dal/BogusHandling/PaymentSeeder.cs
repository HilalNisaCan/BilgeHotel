using Bogus;
using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.BogusHandling
{
    /// <summary>
    /// PaymentSeeder, her rezervasyon için sahte ödeme verisi oluşturur.
    /// Ödeme bilgileri rezervasyonun toplam ücretine göre üretilir.
    /// </summary>
    public static class PaymentSeeder
    {
        /// <summary>
        /// Eğer veritabanında ödeme verisi yoksa, sahte ödemeleri oluşturup ekler.
        /// </summary>
        public static async Task SeedAsync(MyContext context)
        {
            if (context.Payments.Any())
                return;

            // Rezervasyonlar, müşteri ve kullanıcı bilgileriyle birlikte çekilir
            // Sadece Customer ve User'ı olan rezervasyonları al
            List<Reservation> reservations = await context.Reservations
                .Include(r => r.Customer)
                    .ThenInclude(c => c.User)
                .Where(r => r.Customer != null && r.Customer.UserId != null)
                .ToListAsync();

            if (!reservations.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ [PaymentSeeder] Hiç geçerli rezervasyon bulunamadı. Ödeme oluşturulmadı.");
                Console.ResetColor();
                return;
            }

            List<Payment> payments = new List<Payment>();
            Random random = new Random();

            foreach (Reservation reservation in reservations)
            {
                // %50–100 arası ödeme yapılmış gibi gösteriyoruz
                decimal paidAmount = reservation.TotalPrice * (decimal)(random.NextDouble() * 0.5 + 0.5);

                Payment payment = new Payment
                {
                    ReservationId = reservation.Id,
                    CustomerId = reservation.CustomerId,
                    UserId = reservation.Customer.UserId,
                    TotalAmount = reservation.TotalPrice,
                    PaidAmount = paidAmount,
                    ExchangeRate = reservation.ExchangeRate,
                    PaymentStatus = paidAmount >= reservation.TotalPrice ? PaymentStatus.Completed : PaymentStatus.Pending,
                    PaymentMethod = (PaymentMethod)random.Next(0, Enum.GetNames(typeof(PaymentMethod)).Length),
                    InvoiceNumber = $"INV-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                    TransactionId = Guid.NewGuid().ToString(),
                    CancellationReason = "N/A",
                    IsAdvancePayment = false,
                    IsRefunded = false,
                    Description = $"Rezervasyon ödemesi - {reservation.Id}",
                    CreatedDate = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Status = DataStatus.Inserted
                };

                payments.Add(payment);
            }

            context.Payments.AddRange(payments);
            await context.SaveChangesAsync();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("💸 PaymentSeeder → Tüm sahte ödemeler başarıyla eklendi.");
            Console.ResetColor();
        }
    }
}
