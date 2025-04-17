using Project.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.RoomTypePrice
{
    /// <summary>
    /// Yeni bir oda tipi fiyatı oluşturmak için kullanılan istek modeli.
    /// </summary>
    public class CreateRoomTypePriceRequestModel
    {
        [Required]
        public RoomType RoomType { get; set; }

        [Required]
        [Range(1, 99999)]
        public decimal PricePerNight { get; set; }
    }
}
