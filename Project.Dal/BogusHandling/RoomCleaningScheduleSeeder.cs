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
    /// Oda temizlik planlamalarını sahte verilerle oluşturur.
    /// Her oda için 5 adet temizlik planı eklenir.
    /// </summary>
    public static class RoomCleaningScheduleSeeder
    {
        /// <summary>
        /// Veritabanında RoomCleaningSchedule tablosu boşsa, temizlik planlarını üretip ekler.
        /// </summary>
        /// <param name="context">Veritabanı bağlamı</param>
        public static async Task SeedAsync(MyContext context)
        {
            // Tekrarlamayı önlemek için önce veri kontrolü yapılır
            if (context.RoomCleaningSchedules.Any())
                return;

            Faker faker = new Faker("en");

            List<RoomCleaningSchedule> cleaningSchedules = new List<RoomCleaningSchedule>();
            List<Room> rooms = context.Rooms.ToList();

            // Her oda için 5 temizlik planı oluşturuluyor
            foreach (Room room in rooms)
            {
                for (int i = 0; i < 5; i++)
                {
                    RoomCleaningSchedule schedule = new RoomCleaningSchedule
                    {
                        RoomId = room.Id,

                        // Gelecekteki rastgele bir tarihe temizlik planlanır
                        ScheduledDate = faker.Date.Between(DateTime.Now, DateTime.Now.AddMonths(6)),

                        // Rastgele temizlik durumu (Planned, Completed, Delayed vs.)
                        CleaningStatus = faker.PickRandom<CleaningStatus>(),

                        // Açıklama (faker ile üretilmiş kısa cümle)
                        Description = faker.Lorem.Sentence(),

                        // Temizlik ataması için rastgele bir çalışan ID’si
                        AssignedEmployeeId = faker.Random.Int(1, 10),

                        // Temizlik tamamlandı mı?
                        IsCompleted = faker.Random.Bool(),

                        CreatedDate = DateTime.Now
                    };

                    cleaningSchedules.Add(schedule);
                }
            }

            context.RoomCleaningSchedules.AddRange(cleaningSchedules);
            await context.SaveChangesAsync();
        }
    }


    /*Açıklamalar:
Bu seed, odaların temizlik planlarını oluşturur.

Temizlik tarihleri ve durumu rastgele belirlenir.

Temizlik görevini atanan çalışan da rastgele seçilir.

*/
}
