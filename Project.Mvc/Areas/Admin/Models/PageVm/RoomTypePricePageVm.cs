using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.RoomTypePrice;

namespace Project.MvcUI.Areas.Admin.Models.PageVm
{
    public class RoomTypePricePageVm
    {
        public List<RoomTypePriceDto> RoomTypePrices { get; set; }
        public CreateRoomTypePriceRequestModel NewPrice { get; set; }

        public RoomType? SelectedRoomType { get; set; } // Filtreleme gibi işlemler için
    }
}
