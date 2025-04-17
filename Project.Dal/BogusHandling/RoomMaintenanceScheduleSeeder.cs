using Bogus;
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
    /// Oda bakım planlarının sahte verilerle oluşturulmasını sağlayan seeder sınıfıdır.
    /// Her oda için rastgele 5 farklı bakım planı üretilir.
    /// </summary>
    public static class RoomMaintenanceScheduleSeeder
    {
        /// <summary>
        /// Eğer sistemde hiç bakım kaydı yoksa, her oda için 5 adet bakım planı üretip veritabanına ekler.
        /// </summary>
        /// <param name="context">Veritabanı bağlamı</param>
        public static async Task SeedAsync(MyContext context)
        {
            // Eğer veritabanında zaten bakım kaydı varsa, seedleme yapılmaz
            if (context.RoomMaintenances.Any())
                return;

            Faker faker = new Faker("tr");
            List<RoomMaintenance> maintenanceList = new List<RoomMaintenance>();

            // Tüm mevcut odalar veritabanından alınır
            List<Room> rooms = context.Rooms.ToList();

            // Her oda için 5 adet bakım kaydı oluşturulur
            foreach (Room room in rooms)
            {
                for (int i = 0; i < 5; i++)
                {
                    RoomMaintenance maintenance = new RoomMaintenance
                    {
                        RoomId = room.Id,

                        // Rastgele bakım tipi (Elektrik, Temizlik, Donanım vs.)
                        MaintenanceType = faker.PickRandom<MaintenanceType>(),

                        // Rastgele ileri bir tarih (şu an ile 6 ay sonrası arasında)
                        ScheduledDate = faker.Date.Between(DateTime.Now, DateTime.Now.AddMonths(6)),

                        // Sabit bir bakım süresi: şimdi + 2 saat
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddHours(2),

                        // Bakım açıklaması
                        Description = faker.Lorem.Sentence(),

                        // Rastgele bakım durumu
                        MaintenanceStatus = faker.PickRandom<MaintenanceStatus>(),

                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    maintenanceList.Add(maintenance);
                }
            }

            // Üretilen tüm bakım planları veritabanına eklenir
            context.RoomMaintenances.AddRange(maintenanceList);
            await context.SaveChangesAsync();
        }
    }
}