using AutoMapper;
using Project.BLL.DtoClasses;
using Project.Entities.Models;
using Project.MvcUI.Models.PureVm.RequestModel.User;

namespace Project.MvcUI.VmMapping
{
    public class UserVmProfile:Profile
    {
        public UserVmProfile()
        {
            CreateMap<RegisterViewModel, User>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<RegisterViewModel, UserProfile>()
               .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
               .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

            CreateMap<RegisterViewModel, KimlikBilgisiDto>()
          .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
          .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
          .ForMember(dest => dest.IdentityNumber, opt => opt.MapFrom(src => src.IdentityNumber))
          .ForMember(dest => dest.BirthYear, opt => opt.MapFrom(src => src.BirthDate.Year))
          .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
        }
    }
}
