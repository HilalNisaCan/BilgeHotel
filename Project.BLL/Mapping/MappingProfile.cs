using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Interfaces;
using Project.Entities.Models;


namespace Project.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> DTO Dönüşümleri
            CreateMap<Reservation, ReservationDto>()
           .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
           .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room)).ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<Room, RoomDto>()
           .ForMember(dest => dest.RoomImages, opt => opt.MapFrom(src => src.RoomImages)).ReverseMap();


            CreateMap<Employee, EmployeeDto>()
      .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
      .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)) // ✅ DÜZELTİLDİ
      .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName)).ReverseMap();

            CreateMap<Payment, PaymentDto>().ReverseMap();

            CreateMap<BackupLog, BackupLogDto>()
          .ForMember(dest => dest.UserFullName,
                     opt => opt.MapFrom(src => src.User.UserProfile.FirstName + " " + src.User.UserProfile.LastName))
          .ReverseMap()
          .ForMember(dest => dest.User, opt => opt.Ignore())         // 🔒 navigation property ignore
          .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)); // ✅ FK alanı manuel eşlensin




            CreateMap<Campaign, CampaignDto>().ReverseMap();
            CreateMap<ComplaintLog, ComplaintLogDto>().ReverseMap();
            CreateMap<Customer, CustomerDto>()
      .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
      .ReverseMap();
            CreateMap<EarlyReservationDiscount, EarlyReservationDiscountDto>().ReverseMap();
            CreateMap<EmployeeShift, EmployeeShiftDto>()
     .ForMember(dest => dest.AssignedEmployees, opt => opt.MapFrom(src => src.ShiftAssignments)) // 🟢 Burası çok kritik
     .ReverseMap();
            CreateMap<ExchangeRate, ExchangeRateDto>().ReverseMap();
            CreateMap<GuestVisitLog, GuestVisitLogDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ReportLog, ReportLogDto>()
          .ForMember(dest => dest.ReportDate, opt => opt.MapFrom(src => src.ReportDate))
          .ForMember(dest => dest.ReportType, opt => opt.MapFrom(src => src.ReportType))
          .ForMember(dest => dest.ReportStatus, opt => opt.MapFrom(src => src.ReportStatus))
          .ForMember(dest => dest.LogMessage, opt => opt.MapFrom(src => src.LogMessage))
          .ForMember(dest => dest.ReportData, opt => opt.MapFrom(src => src.ReportData))
          .ForMember(dest => dest.XmlFilePath, opt => opt.MapFrom(src => src.XmlFilePath))
          .ForMember(dest => dest.IsSystemGenerated, opt => opt.MapFrom(src => src.IsSystemGenerated))
          .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
          .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
          .ForMember(dest => dest.IPAddress, opt => opt.MapFrom(src => src.IPAddress))
          .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.ErrorMessage))
          .ReverseMap();

            CreateMap<Review, ReviewDto>()
      .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.User.UserProfile.FirstName))
      .ForMember(dest => dest.UserLastName, opt => opt.MapFrom(src => src.User.UserProfile.LastName));

            CreateMap<RoomMaintenanceAssignment, RoomMaintenanceAssignmentDto>()
                .ForMember(dest => dest.AssignedEmployeeFullName,
                           opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
                .ReverseMap()
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForMember(dest => dest.Room, opt => opt.Ignore())
                .ForMember(dest => dest.RoomMaintenance, opt => opt.Ignore())
                .ForMember(dest => dest.MaintenanceStatus,
                           opt => opt.MapFrom(src => src.MaintenanceStatus ?? MaintenanceStatus.Scheduled));

            CreateMap<RoomCleaningSchedule, RoomCleaningScheduleDto>()
      .ForMember(dest => dest.AssignedEmployeeFullName,
                 opt => opt.MapFrom(src => src.AssignedEmployee.FirstName + " " + src.AssignedEmployee.LastName))
      .ForMember(dest => dest.Status,
                 opt => opt.MapFrom(src => src.Status))
      .ReverseMap()
      .ForMember(dest => dest.Status,
                 opt => opt.MapFrom(src => src.Status)); // ✅ DTO'dan Entity'ye geri dönüş



            CreateMap<RoomImage, RoomImageDto>().ReverseMap();

            CreateMap<RoomMaintenance, RoomMaintenanceDto>().ReverseMap();

            CreateMap<EmployeeShiftAssignmentDto, EmployeeShiftAssignment>()
     .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
     .ForMember(dest => dest.EmployeeShiftId, opt => opt.MapFrom(src => src.EmployeeShiftId))
     .ForMember(dest => dest.AssignedDate, opt => opt.MapFrom(src => src.AssignedDate))
     .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
     .ForMember(dest => dest.Employee, opt => opt.Ignore())
     .ForMember(dest => dest.EmployeeShift, opt => opt.Ignore())
     .ForMember(dest => dest.Id, opt => opt.Ignore())
     .ForMember(dest => dest.Status, opt => opt.Ignore())
     .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
     .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
     .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
     .ForMember(dest => dest.ShiftStatus, opt => opt.Ignore())
     .ReverseMap();



            CreateMap<ExtraExpense, ExtraExpenseDto>()
     .ForMember(dest => dest.CustomerFullName,
         opt => opt.MapFrom(src =>
             src.Customer.User.UserProfile.FirstName + " " + src.Customer.User.UserProfile.LastName))
     .ForMember(dest => dest.ProductName,
         opt => opt.MapFrom(src => src.Product.Name))
     .ForMember(dest => dest.ReservationInfo,
         opt => opt.MapFrom(src => $"Oda {src.Reservation.Room.RoomNumber}"));

            CreateMap<ExtraExpenseDto, ExtraExpense>()
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Reservation, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.Payment, opt => opt.Ignore());


            CreateMap<UserProfile, UserProfileDto>().ReverseMap();
            CreateMap<RoomTypePriceDto, RoomTypePrice>().ReverseMap();
            CreateMap<Campaign, CampaignDto>()
           .ForMember(dest => dest.ProductImagePath, opt => opt.MapFrom(src => src.ProductImagePath ?? ""));






        }
    }
}
