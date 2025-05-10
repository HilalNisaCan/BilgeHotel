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
    /// </summary>
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
            int roomNumber = 101;

            // KAT 1: 10 Tek kişilik, 10 Üç kişilik oda
            rooms.AddRange(CreateRooms(1, RoomType.Single, 10, ref roomNumber));
            rooms.AddRange(CreateRooms(1, RoomType.Triple, 10, ref roomNumber));

            // KAT 2: 10 Tek kişilik, 10 Twin oda
            rooms.AddRange(CreateRooms(2, RoomType.Single, 10, ref roomNumber));
            rooms.AddRange(CreateRooms(2, RoomType.TwinBed, 10, ref roomNumber));

            // KAT 3: 10 Double, 10 Triple
            rooms.AddRange(CreateRooms(3, RoomType.DoubleBed, 10, ref roomNumber));
            rooms.AddRange(CreateRooms(3, RoomType.Triple, 10, ref roomNumber));

            // KAT 4: 10 Double, 6 Quad, 1 KingSuite
            rooms.AddRange(CreateRooms(4, RoomType.DoubleBed, 10, ref roomNumber));
            rooms.AddRange(CreateRooms(4, RoomType.Quad, 6, ref roomNumber));
            rooms.AddRange(CreateRooms(4, RoomType.KingSuite, 1, ref roomNumber));

            return rooms;
        }

        /// <summary>
        /// Verilen parametrelere göre oda listesi oluşturur.
        /// </summary>
        private static List<Room> CreateRooms(int floor, RoomType roomType, int count, ref int roomNumber)
        {
            List<Room> list = new List<Room>();

            for (int i = 0; i < count; i++)
            {
                bool hasBalcony = floor >= 3;
                bool hasMinibar = roomType != RoomType.Single;

                Room room = new Room
                {
                    RoomNumber = roomNumber.ToString(),
                    FloorNumber = floor,
                    RoomType = roomType,
                    Capacity = GetCapacity(roomType),
                    HasBalcony = hasBalcony,
                    HasMinibar = hasMinibar,
                    HasAirConditioning = true,
                    HasTV = true,
                    HasHairDryer = true,
                    HasWirelessInternet = true,
                    PricePerNight = GetFixedPrice(roomType),
                    IsCleaned = true,
                    RoomStatus = RoomStatus.Available,
                    Description = GenerateRoomDescription(roomType, floor, hasMinibar, hasBalcony),
                    CreatedDate = DateTime.Now
                };

                list.Add(room);
                roomNumber++;
            }

            return list;
        }

        /// <summary>
        /// Oda tipine göre kapasite belirlenir.
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
                RoomType.KingSuite => 2,
                _ => 1
            };
        }

        /// <summary>
        /// Oda tipine göre sabit gecelik fiyat belirlenir.
        /// </summary>
        private static decimal GetFixedPrice(RoomType type)
        {
            return type switch
            {
                RoomType.Single => 1700,
                RoomType.DoubleBed => 2000,
                RoomType.TwinBed => 2100,
                RoomType.Triple => 2500,
                RoomType.Quad => 3000,
                RoomType.KingSuite => 5000,
                _ => 1000
            };
        }

        /// <summary>
        /// Oda tipi ve kata göre açıklama metni üretir.
        /// </summary>
        private static string GenerateRoomDescription(RoomType roomType, int floor, bool hasMinibar, bool hasBalcony)
        {
            List<string> lines = new List<string>();

            string baseText = roomType switch
            {
                RoomType.Single => "Konforlu tek kişilik odamız, sade ve şık tasarımıyla sessiz bir konaklama sunar.",
                RoomType.TwinBed => "İki ayrı yataklı odamız, arkadaşlar ve iş seyahati yapan çiftler için idealdir.",
                RoomType.DoubleBed => "Geniş çift kişilik yatağıyla konfor sunan odamız, çiftler için idealdir.",
                RoomType.Triple => "Aileler veya arkadaş grupları için tasarlanan üç kişilik odalarımız, ferah yapısıyla öne çıkar.",
                RoomType.Quad => "Dört kişilik odalarımız, aileler için mükemmel bir seçenektir.",
                RoomType.KingSuite => "Kral dairemiz; geniş yatak, balkon ve lüks tasarımıyla en üst düzeyde konfor sağlar.",
                _ => "Standart odamız konforlu ve rahattır."
            };

            lines.Add(baseText);
            lines.Add("Klima, TV, saç kurutma makinesi ve kablosuz internet standarttır.");

            if (hasMinibar)
                lines.Add("Minibar mevcuttur.");

            if (hasBalcony)
                lines.Add("Bu oda balkona sahiptir.");

            return string.Join(" ", lines);
        }
    }
}