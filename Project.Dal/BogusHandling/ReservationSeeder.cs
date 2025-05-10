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
            List<Room> rooms = context.Rooms.ToList();

            if (!customerIds.Any() || !rooms.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ [ReservationSeeder] Müşteri veya oda yok. Rezervasyon oluşturulmadı.");
                Console.ResetColor();
                return;
            }

            List<Reservation> reservations = GenerateReservations(15, customerIds, rooms);
            context.Reservations.AddRange(reservations);
            await context.SaveChangesAsync();
        }

        public static List<Reservation> GenerateReservations(int count, List<int> customerIds, List<Room> rooms)
        {
            Faker faker = new Faker("en");
            List<Reservation> reservations = new List<Reservation>();

            for (int i = 0; i < count; i++)
            {
                DateTime startDate = faker.Date.FutureOffset(1).Date;
                DateTime endDate = startDate.AddDays(faker.Random.Int(1, 7));
                DateTime reservationDate = DateTime.Now.AddDays(-faker.Random.Int(1, 60));
                TimeSpan checkInTime = new TimeSpan(14, 0, 0);

                Room room = faker.PickRandom(rooms);
                int nights = (endDate - startDate).Days;
                decimal pricePerNight = room.PricePerNight;
                decimal totalPrice = pricePerNight * nights;

                ReservationPackage package = faker.PickRandom<ReservationPackage>();
                int daysBefore = (startDate - reservationDate).Days;
                decimal discount = 0;

                if (daysBefore >= 90)
                    discount = 23;
                else if (daysBefore >= 30)
                    discount = (package == ReservationPackage.AllInclusive) ? 18 : 16;

                decimal discountedPrice = totalPrice * (1 - discount / 100m);

                Reservation reservation = new Reservation
                {
                    CustomerId = faker.PickRandom(customerIds),
                    RoomId = room.Id,
                    ReservationDate = reservationDate,
                    StartDate = startDate,
                    EndDate = endDate,
                    CheckInTime = checkInTime,
                    NumberOfGuests = faker.Random.Int(1, 4),
                    Package = package,
                    ExchangeRate = faker.Random.Decimal(22, 30),
                    CurrencyCode = "TRY",
                    DiscountRate = discount,
                    TotalPrice = discountedPrice,
                    ReservationStatus = ReservationStatus.Confirmed,
                    CreatedDate = DateTime.Now
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
