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
    /// RoomSeeder, Bilge Hotel’in odalarını kat ve tip dağılımına göre sahte verilerle doldurmak için kullanılır.
    /// Bu sınıf, otelin kat bazlı kurallarını birebir uygular.
    public static class RoomSeeder
    {
        /// <summary>
        /// Veritabanına odaları yalnızca bir kez ekler.
        /// </summary>
        public static async Task SeedAsync(MyContext context)
        {
            if (!context.Rooms.Any())
            {
                List<Room> rooms = SeedRooms();
                context.Rooms.AddRange(rooms);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Otelin tüm kat ve oda yapısına uygun şekilde sahte odaları üretir.
        /// </summary>
        public static List<Room> SeedRooms()
        {
            List<Room> rooms = new List<Room>();
            int roomNumber = 100;

            // KAT 1: 10 Tek kişilik, 10 Üç kişilik oda
            rooms.AddRange(CreateRooms(1, RoomType.Single, 10, ref roomNumber));
            rooms.AddRange(CreateRooms(1, RoomType.Triple, 10, ref roomNumber));

            // KAT 2: 10 Tek kişilik, 10 İki kişilik (çift yataklı)
            rooms.AddRange(CreateRooms(2, RoomType.Single, 10, ref roomNumber));
            rooms.AddRange(CreateRooms(2, RoomType.TwinBed, 10, ref roomNumber));

            // KAT 3: 10 İki kişilik (tek yataklı), 10 Üç kişilik
            rooms.AddRange(CreateRooms(3, RoomType.DoubleBed, 10, ref roomNumber));
            rooms.AddRange(CreateRooms(3, RoomType.Triple, 10, ref roomNumber));

            // KAT 4: 10 İki kişilik (tek yataklı), 6 Dört kişilik, 1 Kral Dairesi
            rooms.AddRange(CreateRooms(4, RoomType.DoubleBed, 10, ref roomNumber));
            rooms.AddRange(CreateRooms(4, RoomType.Quad, 6, ref roomNumber));
            rooms.AddRange(CreateRooms(4, RoomType.KingSuite, 1, ref roomNumber));

            return rooms;
        }

        /// <summary>
        /// Verilen parametrelere göre oda listesi oluşturur.
        /// Kat numarasına göre balkon, oda tipine göre minibar gibi kurallar uygulanır.
        /// Açıklama metni dinamik olarak oluşturulur.
        /// </summary>
        private static List<Room> CreateRooms(int floor, RoomType roomType, int count, ref int roomNumber)
        {
            List<Room> list = new List<Room>();

            for (int i = 0; i < count; i++)
            {
                Room room = new Room
                {
                    RoomNumber = roomNumber.ToString(),
                    FloorNumber = floor,
                    RoomType = roomType,
                    Capacity = GetCapacity(roomType),
                    HasBalcony = floor >= 3,
                    HasMinibar = roomType != RoomType.Single,
                    HasAirConditioning = true,
                    HasTV = true,
                    HasHairDryer = true,
                    HasWirelessInternet = true,
                    PricePerNight = GetFixedPrice(roomType),
                    IsCleaned = true,
                    RoomStatus = RoomStatus.Available,
                    Description = GenerateRoomDescription(roomType, floor), // ⬅️ açıklama eklendi
                    CreatedDate = DateTime.Now,
                    ModifiedDate = null,
                    DeletedDate = null
                };

                list.Add(room);
                roomNumber++;
            }

            return list;
        }

        /// <summary>
        /// Oda tipine göre kaç kişi kalabileceğini döndürür.
        /// </summary>
        private static int GetCapacity(RoomType type)
        {
            return type switch
            {
                RoomType.Single => 1,
                RoomType.DoubleBed => 2,
                RoomType.TwinBed => 2,
                RoomType.Triple => 3,
                RoomType.Quad => 4,
                RoomType.KingSuite => 5,
                _ => 1
            };
        }

        /// <summary>
        /// Oda tipine göre sabit gecelik fiyat döner.
        /// </summary>
        private static decimal GetFixedPrice(RoomType type)
        {
            return type switch
            {
                RoomType.Single => 1500,
                RoomType.DoubleBed => 2000,
                RoomType.TwinBed => 2100,
                RoomType.Triple => 2500,
                RoomType.Quad => 3000,
                RoomType.KingSuite => 5000,
                _ => 1000
            };
        }

        /// <summary>
        /// Oda tipi ve kata göre açıklama metni oluşturur.
        /// </summary>
        private static string GenerateRoomDescription(RoomType roomType, int floor)
        {
            List<string> lines = new List<string>();

            lines.Add("Odamızda klima, TV, saç kurutma makinesi ve kablosuz internet standart olarak sunulmaktadır.");

            if (roomType != RoomType.Single)
                lines.Add("Minibar mevcuttur.");

            if (floor >= 3)
                lines.Add("Bu oda balkona sahiptir.");

            return string.Join(" ", lines);
        }
    }
}