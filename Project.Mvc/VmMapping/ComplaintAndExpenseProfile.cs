using AutoMapper;
using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Complaint;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Complaint;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Review;
using Project.MvcUI.Areas.Reservation.Models.PageVm;
using Project.MvcUI.Areas.Reservation.Models.PureVm.ResponseModel.ExtraExpense;
using Project.MvcUI.Models.PureVm.RequestModel.Contact;

namespace Project.MvcUI.VmMapping
{
    public class ComplaintAndExpenseProfile : Profile
    {
        public ComplaintAndExpenseProfile()
        {
            // ------------------- 📢 COMPLAINT -------------------

            CreateMap<ComplaintLogDto, ComplaintLogResponseModel>()
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ReverseMap();

            CreateMap<ComplaintReplyRequestModel, ComplaintLogDto>().ReverseMap();

            CreateMap<ContactFormRequestModel, ComplaintLogDto>()
           .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
           .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => "İletişim Formu")) // sabit değer
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Message))
           .ForMember(dest => dest.SubmittedDate, opt => opt.MapFrom(_ => DateTime.Now))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => ComplaintStatus.Pending))
           .ForMember(dest => dest.IsResolved, opt => opt.MapFrom(_ => false))
           .ForMember(dest => dest.Response, opt => opt.MapFrom(_ => string.Empty));

            // ------------------- 💸 EXTRA EXPENSE -------------------

            CreateMap<ExtraExpenseDto, ExtraExpenseModel>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.ExpenseDate ?? DateTime.MinValue));

            CreateMap<AddExtraExpenseModel, ExtraExpenseDto>()
           .ForMember(dest => dest.CustomerId, opt => opt.Ignore()) // Reservation üzerinden atanacak
           .ForMember(dest => dest.UnitPrice, opt => opt.Ignore())  // Product üzerinden alınacak
          .ForMember(dest => dest.Description, opt => opt.Ignore()); // Ürün adı manuel atanacak


            //--------------------- Review---------------------------//
            CreateMap<ReviewDto, ReviewAdminResponseModel>()
          .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.UserFirstName + " " + src.UserLastName));


         
        }
    }

}
