using Microsoft.AspNetCore.Identity;
using Project.Dal.ContextClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Dal.BogusHandling.SeederManager
{/// <summary>
 /// Tüm sahte verilerin oluşturulmasını yöneten merkezi seeder sınıfıdır.
 /// Projedeki 13 seeder sınıfı burada sırayla tetiklenir.
 /// </summary>
    public static class SeederManager
    {
        public static async Task SeedAllAsync(MyContext context, UserManager<User> userManager)
        {
            Console.WriteLine("📦 Seed işlemi başlatıldı...\n");
            await EmployeeSeeder.SeedAsync(context);                     // 1️⃣ Çalışanlar (ilk! çünkü AppUser ile eşleşecek)
            AppUserSeeder appUserSeeder = new AppUserSeeder(userManager, context);
            await appUserSeeder.SeedAsync(context.Employees.ToList());   // 2️⃣ AppUser (Yönetici + Resepsiyonist) → Employee.UserId atanıyor
            await CustomerSeeder.SeedAsync(context);                     // 3️⃣ Customer (Customer rolündeki kullanıcılar için)

            await RoomSeeder.SeedAsync(context);                         // 4️⃣ Oda verileri (Room)
            await RoomImageSeeder.SeedAsync(context);                    // 5️⃣ Oda görselleri (RoomId)
            await RoomTypePriceSeeder.SeedAsync(context);                // 6️⃣ Oda fiyatları (RoomType → enum bağımlı)

            await EmployeeShiftSeeder.SeedAsync(context);                // 7️⃣ Vardiyalar (vardiya tipleri ve saat aralıkları)
            await EmployeeShiftAssignmentSeeder.SeedAsync(context);      // 8️⃣ Vardiya atamaları (Shift + Employee)

            await ReservationSeeder.SeedAsync(context);                  // 9️⃣ Rezervasyonlar (Customer + Room bağlı)
            await PaymentSeeder.SeedAsync(context);                      // 🔟 Ödemeler (Reservation ve Customer bağlı)

            await ProductSeeder.SeedAsync(context);                      // 1️⃣1️⃣ Ürünler (minibar, spa vs.)
            await OrderSeeder.SeedAsync(context);                        // 1️⃣2️⃣ Siparişler (Payment ve Product bağlı)

            await RoomCleaningScheduleSeeder.SeedAsync(context);         // 1️⃣3️⃣ Temizlik planı (oda rezervasyonlarına göre)
            await RoomMaintenanceScheduleSeeder.SeedAsync(context);     // 1️⃣4️⃣ Bakım planı

            Console.WriteLine("\n✅ Tüm seed işlemleri tamamlandı!");
        }
    }


}   /*m bu sınıf projenin tüm sahte verilerini oluşturuyor.
Sadece SeedAllAsync() metodunu çağırmam yeterli.
Sıralı olarak 13 farklı tabloya mantıklı veriler ekleniyor.
Console logları sayesinde süreç profesyonelce takip edilebiliyor.”*/

