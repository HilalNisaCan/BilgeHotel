namespace Project.MvcUI.Models.PageVm.Room
{
    public class RoomListVm
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public string RoomTypeName { get; set; }
        public string RoomStatusName { get; set; }
        public decimal Price { get; set; }
        public string Floor { get; set; }
        public bool HasMiniBar { get; set; }
        public bool HasBalcony { get; set; }
    }
}