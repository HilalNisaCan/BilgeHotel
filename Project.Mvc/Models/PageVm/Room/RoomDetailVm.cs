using Project.Entities.Enums;

namespace Project.MvcUI.Models.PageVm.Room
{
    public class RoomDetailVm
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public RoomType RoomType { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Floor { get; set; }
        public bool HasMiniBar { get; set; }
        public bool HasBalcony { get; set; }
        public IEnumerable<string> Images { get; set; } // Oda resimleri için
    }
}
