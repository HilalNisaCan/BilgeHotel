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
/// RoomImageSeeder, her oda için uygun oda tipine göre görselleri RoomImage tablosuna ekler.
/// Her odada yalnızca bir kapak resmi (IsMain = true) atanır.
/// </summary>
public static class RoomImageSeeder
    {
        public static async Task SeedAsync(MyContext context)
        {
            if (!context.RoomImages.Any())
            {
                List<RoomImage> imageList = new List<RoomImage>();

                // Tüm odaları al
                List<Room> rooms = await context.Rooms.ToListAsync();

                foreach (Room room in rooms)
                {
                    // Oda tipine göre görsel listesini al
                    List<string> imagePaths = GetImagePathsByRoomType(room.RoomType);

                    for (int i = 0; i < imagePaths.Count; i++)
                    {
                        RoomImage image = new RoomImage
                        {
                            RoomId = room.Id,
                            ImagePath = imagePaths[i].ToLowerInvariant(), // Dosya adını küçük harfe çevir
                            IsMain = i == 0, // İlk görsel kapak olsun
                            CreatedDate = DateTime.Now,
                            ModifiedDate = null,
                            DeletedDate = null,
                            Status = DataStatus.Inserted
                        };

                        imageList.Add(image);
                    }
                }

                await context.RoomImages.AddRangeAsync(imageList);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Oda tipine göre ilgili görsel adlarını döndürür.
        /// </summary>
        private static List<string> GetImagePathsByRoomType(RoomType type)
        {
            switch (type)
            {
                case RoomType.Single:
                    return new List<string> { "single-1.jpg", "single-2.jpg", "single-3.jpg" };

                case RoomType.DoubleBed:
                    return new List<string> { "double-1.jpg", "double-2.jpg", "double-3.jpg" };

                case RoomType.TwinBed:
                    return new List<string> { "twin-1.jpg", "twin-2.jpg", "twin-3.jpg" };

                case RoomType.Triple:
                    return new List<string> { "triple-1.jpg", "triple-2.jpg", "triple-3.jpg", "triple-4.jpg" };

                case RoomType.Quad:
                    return new List<string> { "quad-1.jpg", "quad-2.jpg", "quad-3.jpg" };

                case RoomType.KingSuite:
                    return new List<string> { "king-1.jpg", "king-2.jpg", "king-3.jpg", "king-4.jpg", "king-5.jpg" };

                default:
                    return new List<string> { "default.jpg" };
            }
        }
    }
}
