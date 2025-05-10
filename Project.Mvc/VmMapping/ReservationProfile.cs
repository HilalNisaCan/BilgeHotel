using AutoMapper;
using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PageVm;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Reservation;
using Project.MvcUI.Areas.Reservation.Models.PageVm;
using Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Reservation;
using Project.MvcUI.Models.PageVm.Reservation;
using Project.MvcUI.Models.PureVm.RequestModel.Reservation;
using Project.MvcUI.PaymentApiTools;

namespace Project.MvcUI.VmMapping
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            ShouldMapProperty = p => true;

            // ------------------- 🏨 RESERVATION -------------------
            CreateMap<Reservation, ReservationCheckInOutModel>()
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => $"{src.Customer.FirstName} {src.Customer.LastName}"))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.ReservationStatus, opt => opt.MapFrom(src => src.ReservationStatus))
                .ForMember(dest => dest.Package, opt => opt.MapFrom(src => src.Package));

            CreateMap<ReservationDto, ReservationCheckInOutModel>()
          .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => $"{src.Customer.FirstName} {src.Customer.LastName}"))
          .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
          .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
          .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
          .ForMember(dest => dest.Package, opt => opt.MapFrom(src => src.Package))
          .ForMember(dest => dest.ReservationStatus, opt => opt.MapFrom(src => src.ReservationStatus));


            CreateMap<ReservationDto, CheckOutDetailModel>()
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => $"{src.Customer.FirstName} {src.Customer.LastName}"))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));

            CreateMap<CreateReservationRequestModel, ReservationDto>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CheckIn))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.CheckOut))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.Package, opt => opt.MapFrom(src => src.Package))
                .ForMember(dest => dest.NumberOfGuests, opt => opt.MapFrom(src => src.GuestCount))
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<CreateReservationRequestModel, CustomerDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LoyaltyPoints, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.IsIdentityVerified, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => (int?)null))
                .ForMember(dest => dest.BillingDetails, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.BillingDetails) ? "Standart Bireysel Fatura" : src.BillingDetails))
                .ForMember(dest => dest.NeedsIdentityCheck, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.IdentityNumber, opt => opt.MapFrom(src => src.IdentityNumber));

            CreateMap<CreateReservationRequestModel, KimlikBilgisiDto>();

            CreateMap<ReservationDto, ReservationAdminResponseModel>()
                .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => $"{src.Customer.FirstName} {src.Customer.LastName}"))
                .ForMember(dest => dest.IdentityNumber, opt => opt.MapFrom(src => src.Customer.IdentityNumber))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
                .ForMember(dest => dest.RoomInfo, opt => opt.MapFrom(src => $"Oda {src.Room.Id} - {src.Room.RoomType}"))
                .ForMember(dest => dest.Package, opt => opt.MapFrom(src => src.Package.ToString()))
                .ForMember(dest => dest.ReservationStatus, opt => opt.MapFrom(src => src.ReservationStatus))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.DiscountRate, opt => opt.MapFrom(src => src.DiscountRate))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.CheckInTime, opt => opt.MapFrom(src => src.CheckInTime))
                .ForMember(dest => dest.NumberOfGuests, opt => opt.MapFrom(src => src.NumberOfGuests))
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationDate))
                .ForMember(dest => dest.CampaignName, opt => opt.MapFrom(src => src.CampaignName))
                .ForMember(dest => dest.DiscountRate, opt => opt.MapFrom(src => src.DiscountRate));

            CreateMap<ReservationRequestModel, ReservationTempModel>()
           .ForMember(dest => dest.RoomType, opt => opt.Ignore()) // Enum dönüşümünü manuel yapacağız
           .ForMember(dest => dest.UserId, opt => opt.Ignore())
           .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
           .ForMember(dest => dest.PricePerNight, opt => opt.Ignore())
           .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
           .ForMember(dest => dest.Duration, opt => opt.Ignore())
           .ForMember(dest => dest.DiscountRate, opt => opt.Ignore());

            CreateMap<CustomerReportDto, CustomerReportPageVm>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.PastReservations, opt => opt.MapFrom(src => src.PastReservations))
           .ForMember(dest => dest.UpcomingReservations, opt => opt.MapFrom(src => src.UpcomingReservations));
        }
    }
}
