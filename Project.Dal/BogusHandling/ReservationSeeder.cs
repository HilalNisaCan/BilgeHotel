using Bogus;
using Microsoft.EntityFrameworkCore;
using Project.Dal.ContextClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.BogusHandling
{
   public static class ReservationSeeder
    {
        public static async Task SeedAsync(MyContext context)
        {
            if (context.Reservations.Any())
                return;

            List<int> customerIds = context.Customers.Select(c => c.Id).ToList();
            List<int> roomIds = context.Rooms.Select(r => r.Id).ToList();

            if (!customerIds.Any() || !roomIds.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ [ReservationSeeder] Müşteri veya oda yok. Rezervasyon oluşturulmadı.");
                Console.ResetColor();
                return;
            }


            List<Reservation> reservations = GenerateReservations(15, customerIds, roomIds);
            context.Reservations.AddRange(reservations);
            await context.SaveChangesAsync();
        }

        public static List<Reservation> GenerateReservations(int count, List<int> customerIds, List<int> roomIds)
        {
            Faker faker = new Faker("en");
            List<Reservation> reservations = new List<Reservation>();

            for (int i = 0; i < count; i++)
            {
                DateTime startDate = faker.Date.FutureOffset(1).Date;
                DateTime endDate = startDate.AddDays(faker.Random.Int(1, 7));
                decimal exchangeRate = faker.Random.Decimal(22, 30);

                Reservation reservation = new Reservation
                {
                    CustomerId = faker.PickRandom(customerIds),
                    RoomId = faker.PickRandom(roomIds),
                    ReservationDate = DateTime.Now.AddDays(-faker.Random.Int(1, 10)),
                    StartDate = startDate,
                    EndDate = endDate,
                    CheckInTime = new TimeSpan(14, 0, 0),
                    NumberOfGuests = faker.Random.Int(1, 4),
                    Package = faker.PickRandom<ReservationPackage>(),
                    ExchangeRate = exchangeRate,
                    CurrencyCode = "TRY",
                    DiscountRate = faker.Random.Decimal(0, 25),
                    TotalPrice = faker.Random.Decimal(1500, 6000),
                    ReservationStatus = ReservationStatus.Confirmed,
                    CreatedDate = DateTime.Now,
                 
                };

                reservations.Add(reservation);
            }

            return reservations;
        }
    }


    /*  Tüm rezervasyonlar müşteri, oda ve ödeme tablolarına bağlı. (FK test senaryosu)

  Check-in tarihi gelecekte, konaklama süresi 1–14 gün arası olarak belirleniyor.

  Saat sabit: 14:00 (dökümantasyonla uyumlu)
      Fiyatlar ve indirim oranları gerçekçi aralıklarda.

  CurrencyCode, sistemde döviz bazlı işlem varsa buna hazır olacak şekilde bırakıldı.

  Yorumlar sayesinde her alanın ne işe yaradığını göstermek kolay.*/


}
