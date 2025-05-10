namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Room
{
    public class RoomFilterVm
    {
        public string RoomType { get; set; }
        public string RoomStatus { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? FloorNumber { get; set; }
    }
}
