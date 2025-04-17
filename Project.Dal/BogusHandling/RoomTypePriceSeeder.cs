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
    public static class RoomTypePriceSeeder
    {
        public static async Task SeedAsync(MyContext context)
        {
            if (!context.RoomTypePrices.Any()) // Veritabanında fiyatlar yoksa ekle
            {
                List<RoomTypePrice> prices = new List<RoomTypePrice>
        {
            new RoomTypePrice { RoomType = RoomType.Single, PricePerNight = 1700 },
            new RoomTypePrice { RoomType = RoomType.DoubleBed, PricePerNight = 2000 },
            new RoomTypePrice { RoomType = RoomType.TwinBed, PricePerNight = 2100 },
            new RoomTypePrice { RoomType = RoomType.Triple, PricePerNight = 2500 },
            new RoomTypePrice { RoomType = RoomType.Quad, PricePerNight = 3000 },
            new RoomTypePrice { RoomType = RoomType.KingSuite, PricePerNight = 5000 },
        };

                context.RoomTypePrices.AddRange(prices);
                await context.SaveChangesAsync();  // Veritabanına kaydediyoruz
            }
        }
    }
}
