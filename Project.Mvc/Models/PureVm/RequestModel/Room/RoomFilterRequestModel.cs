using Project.Entities.Enums;

namespace Project.MvcUI.Models.PureVm.RequestModel.Room
{
    public class RoomFilterRequestModel
    {
        public RoomType? RoomType { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? HasBalcony { get; set; }
        public bool? HasMinibar { get; set; }
        public bool? HasWiFi { get; set; }
        public string? Keyword { get; set; } // Kat bilgisi veya oda adı için
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 6; // 6 kart
    }
}
