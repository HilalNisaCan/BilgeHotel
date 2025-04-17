using Project.Entities.Enums;
using Project.MvcUI.Models.PureVm.ResponseModel.Review;
using System.Globalization;

namespace Project.MvcUI.Models.PureVm.ResponseModel.Room
{
    public class RoomResponseModel
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public string RoomNumber { get; set; }
        public string FloorNumber { get; set; }
        public string Capacity { get; set; }

        public RoomType RoomType { get; set; }
        public RoomStatus RoomStatus { get; set; }
        public decimal PricePerNight { get; set; }

        public string ImagePath { get; set; }
        public List<string> ImageGallery { get; set; }

        public bool HasBalcony { get; set; }
        public bool HasMinibar { get; set; }
        public bool HasTV { get; set; }
        public bool HasHairDryer { get; set; }
        public bool HasWiFi { get; set; }

        public string Description { get; set; }

        // 👇 Review alanları
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public List<ReviewDisplayModel> Reviews { get; set; }

    }
}
