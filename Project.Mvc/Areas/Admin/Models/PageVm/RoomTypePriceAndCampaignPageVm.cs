using Project.BLL.DtoClasses;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.RoomTypePrice;

namespace Project.MvcUI.Areas.Admin.Models.PageVm
{
    public class RoomTypePriceAndCampaignPageVm
    {
        public List<RoomTypePriceDto> RoomTypePrices { get; set; }
        public CreateRoomTypePriceRequestModel NewPrice { get; set; }

        public List<CampaignDto> Campaigns { get; set; }
        public CreateCampaignRequestModel NewCampaign { get; set; }
    }
}
