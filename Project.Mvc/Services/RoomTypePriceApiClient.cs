using Project.Entities.Enums;
using System.Text.Json;

namespace Project.MvcUI.Services
{

    /// <summary>
    /// 📡 RoomTypePriceApiClient
    /// 
    /// Bu servis, Web API üzerinden oda tipine göre fiyat bilgisi çeker.
    /// MVC katmanında form doldurma, fiyat güncelleme gibi işlemler için kullanılır.
    /// 
    /// 🎯 Amaç:
    /// - Oda tipine göre fiyatı merkezi bir yerden dinamik olarak almak.
    /// - Fiyatlar yönetici panelinde değişse bile, web sitesine anında yansıtmak.
    /// 
    /// 💡 Kullanım Yeri:
    /// - RoomController → Rezervasyon sayfasında fiyat hesaplamada
    /// - Herhangi bir UI mantığında ViewModel doldurma sürecinde
    /// 
    /// ❗ Bu servis sadece HttpClient üzerinden veri çeker, iş mantığı içermez.
    /// BLL’e değil, sadece UI’ye aittir.
    /// </summary>
    public class RoomTypePriceApiClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// HttpClient bağımlılığı constructor üzerinden alınır.
        /// </summary>
        public RoomTypePriceApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Belirli bir oda tipine göre fiyatı getirir.
        /// Web API'den decimal fiyat bilgisi çekilir.
        /// </summary>
        /// <param name="roomType">Oda tipi enum değeri</param>
        /// <returns>decimal? → fiyat bilgisi, başarısızsa null</returns>
        public async Task<decimal?> GetPriceByRoomTypeAsync(RoomType roomType)
        {
            string url = $"http://localhost:5126/api/RoomTypePrice/price/{roomType}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            string json = await response.Content.ReadAsStringAsync();

            using JsonDocument document = JsonDocument.Parse(json);
            if (document.RootElement.TryGetProperty("price", out JsonElement priceElement))
            {
                return priceElement.GetDecimal();
            }

            return null;
        }
    }
}
