using Project.Entities.Enums;
using System.Text.Json;

namespace Project.MvcUI.Services
{

    public class RoomTypePriceApiClient
    {
        private readonly HttpClient _httpClient;

        public RoomTypePriceApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal?> GetPriceByRoomTypeAsync(RoomType roomType)
        {
            string url = $"http://localhost:5126/api/RoomTypePrice/price/{roomType}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("price", out var priceElement))
            {
                return priceElement.GetDecimal();
            }

            return null;
        }
    }
}
