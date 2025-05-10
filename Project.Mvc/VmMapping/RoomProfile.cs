using AutoMapper;
using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Room;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.RoomTypePrice;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Room;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.RoomTypePrice;
using Project.MvcUI.DependencyResolver.RoomTypePriceResolver;
using Project.MvcUI.Models.PageVm.Room;
using Project.MvcUI.Models.PureVm.ResponseModel.Room;

namespace Project.MvcUI.VmMapping
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            // ------------------- 🖼️ ROOM IMAGE -------------------
            CreateMap<RoomImage, RoomImageDto>().ReverseMap();

            // ------------------- 📋 ROOM → RESPONSE -------------------

            CreateMap<RoomDto, RoomResponseModel>()
                .ForMember(dest => dest.ImagePath,
                    opt => opt.MapFrom(src => src.RoomImages.FirstOrDefault(x => x.IsMain).ImagePath))
                .ForMember(dest => dest.ImageGallery,
                    opt => opt.MapFrom(src => src.RoomImages.Select(x => x.ImagePath).ToList()))
                .ForMember(dest => dest.HasWiFi, opt => opt.MapFrom(src => src.HasWirelessInternet))
                .ForMember(dest => dest.HasTV, opt => opt.MapFrom(src => src.HasTV))
                .ForMember(dest => dest.HasMinibar, opt => opt.MapFrom(src => src.HasMinibar))
                .ForMember(dest => dest.HasHairDryer, opt => opt.MapFrom(src => src.HasHairDryer))
                .ForMember(dest => dest.HasBalcony, opt => opt.MapFrom(src => src.HasBalcony))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<RoomDto, RoomListVm>()
                .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.ToString()))
                .ForMember(dest => dest.RoomStatusName, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePerNight))
                .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.FloorNumber.ToString()))
                .ReverseMap();

            CreateMap<RoomDto, RoomDetailVm>()
                .ForMember(dest => dest.RoomStatus, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePerNight))
                .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.FloorNumber.ToString()))
                .ReverseMap();

            CreateMap<RoomDto, RoomAdminResponseModel>()
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.RoomNumber))
                .ForMember(dest => dest.FloorNumber, opt => opt.MapFrom(src => src.FloorNumber))
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType))
                .ForMember(dest => dest.RoomStatus, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Price, opt => opt.MapFrom<RoomTypePriceResolver>())
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.HasWiFi, opt => opt.MapFrom(src => src.HasWirelessInternet))
                .ForMember(dest => dest.HasTV, opt => opt.MapFrom(src => src.HasTV))
                .ForMember(dest => dest.HasMinibar, opt => opt.MapFrom(src => src.HasMinibar))
                .ForMember(dest => dest.HasHairDryer, opt => opt.MapFrom(src => src.HasHairDryer))
                .ForMember(dest => dest.HasBalcony, opt => opt.MapFrom(src => src.HasBalcony))
                .ForMember(dest => dest.ImageGallery, opt => opt.MapFrom(src => src.RoomImages.Select(x => x.ImagePath).ToList()))
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore())
                .ForMember(dest => dest.CleaningInfo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.ImagePath = src.RoomImages.FirstOrDefault(x => x.IsMain)?.ImagePath;
                });

            // ------------------- ✏️ ROOM ↔️ UPDATE FORM -------------------

            CreateMap<RoomDto, RoomUpdateVm>().ReverseMap();

            // ------------------- 🛠️ MAINTENANCE ASSIGNMENT -------------------

            CreateMap<RoomMaintenanceAssignmentCreateRequestModel, RoomMaintenanceAssignmentDto>()
      .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
      .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
      .ForMember(dest => dest.AssignedDate, opt => opt.MapFrom(src => src.AssignedDate))
      .ForMember(dest => dest.MaintenanceType, opt => opt.MapFrom(src => src.MaintenanceType))
      .ForMember(dest => dest.MaintenanceStatus, opt => opt.MapFrom(src => MaintenanceStatus.Scheduled))
      .ForMember(dest => dest.Status, opt => opt.MapFrom(src => DataStatus.Inserted));


            CreateMap<RoomCleaningScheduleCreateRequestModel, RoomCleaningScheduleDto>()
     .ForMember(dest => dest.AssignedEmployeeId, opt => opt.MapFrom(src => src.AssignedEmployeeId))
     .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
     .ForMember(dest => dest.ScheduledDate, opt => opt.MapFrom(src => src.ScheduledDate))
     .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
     .ForMember(dest => dest.CleaningStatus, opt => opt.MapFrom(src => CleaningStatus.Scheduled))
     .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => false))
     .ForMember(dest => dest.Status, opt => opt.MapFrom(src => DataStatus.Inserted));


            //--------------------------------------------------------//
            // 🟢 ViewModel (ResponseModel) → DTO
            CreateMap<RoomTypePriceResponseModel, RoomTypePriceDto>();

            // 🔄 DTO → ViewModel (string isim + enum)
            CreateMap<RoomTypePriceDto, RoomTypePriceResponseModel>()
                .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.ToString()));

            // 🛠️ Entity → ViewModel (Admin listeleme)
            CreateMap<RoomTypePrice, RoomTypePriceResponseModel>()
                .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.ToString()));

            // 🆕 Create sayfası için: RequestModel → DTO ve Entity
            CreateMap<CreateRoomTypePriceRequestModel, RoomTypePriceDto>().ReverseMap();
            CreateMap<CreateRoomTypePriceRequestModel, RoomTypePrice>();

        }
    }
}
