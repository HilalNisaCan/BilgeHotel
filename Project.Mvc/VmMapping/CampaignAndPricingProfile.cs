using AutoMapper;
using Project.BLL.DtoClasses;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Campaign;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.RoomTypePrice;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Campaign;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.RoomTypePrice;

namespace Project.MvcUI.VmMapping
{
    public class CampaignAndPricingProfile : Profile
    {
        public CampaignAndPricingProfile()
        {
            // ------------------- 🎯 CAMPAIGN -------------------

            CreateMap<CampaignDto, CampaignRequestModel>().ReverseMap(); // ViewModel <-> DTO dönüşümü
            CreateMap<CampaignDto, CampaignResponseModel>(); // DTO → ViewModel dönüşümü (listeleme için)


            // ------------------- 💰 ROOM TYPE PRICE -------------------

            CreateMap<CreateRoomTypePriceRequestModel, RoomTypePrice>(); // ViewModel → Entity

            CreateMap<RoomTypePrice, RoomTypePriceResponseModel>() // Entity → ViewModel dönüşümü
                .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.ToString()));

            CreateMap<RoomTypePriceDto, RoomTypePriceResponseModel>() // DTO → ViewModel dönüşümü
                .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.ToString()));

            CreateMap<CreateRoomTypePriceRequestModel, RoomTypePriceDto>(); // ViewModel → DTO dönüşümü
        }
    }

}
