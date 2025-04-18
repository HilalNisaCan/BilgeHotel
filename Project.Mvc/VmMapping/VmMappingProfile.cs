using AutoMapper;
using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.Entities.Models;
using Project.MvcUI.DependencyResolver.RoomTypePriceResolver;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Employee;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Employee;
using Project.MvcUI.Models.PageVm.Reservation;
using Project.MvcUI.Models.PageVm.Room;
using Project.MvcUI.Models.PureVm.RequestModel.Reservation;
using Project.MvcUI.Models.PureVm.RequestModel.User;
using Project.MvcUI.Models.PureVm.ResponseModel.Room;
using Project.MvcUI.Models.PureVm.ResponseModel.User;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.EmployeeShift;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.EmployeeShift;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Room;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Room;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Reservation;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.RoomTypePrice;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.RoomTypePrice;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Campaign;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Campaign;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Complaint;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Complaint;
using Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Reservation;
using Project.MvcUI.Areas.Reservation.Models.PageVm;
using Project.MvcUI.Areas.Reservation.Models.PureVm.ResponseModel.ExtraExpense;
using Project.MvcUI.Areas.Reservation.Models.PureVm.RequestModel.Register;
using System.Globalization;




namespace Project.MvcUI.VmMapping
{
    public class VmMappingProfile : Profile
    {
     

        public VmMappingProfile() // Yapıcı metod
        {

            ShouldMapProperty = p => true;

          //  CreateMap<ReservationDto, ReservationAdminResponseModel>();

            // Reservation ENTITY → ViewModel map
            CreateMap<Reservation, ReservationCheckInOutModel>()
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => src.Customer.FirstName + " " + src.Customer.LastName))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.ReservationStatus, opt => opt.MapFrom(src => src.ReservationStatus))
                .ForMember(dest => dest.Package, opt => opt.MapFrom(src => src.Package));


            CreateMap<ReservationDto, ReservationCheckInOutModel>()
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => src.Customer.FirstName + " " + src.Customer.LastName))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber));

            CreateMap<ReservationDto, CheckOutDetailModel>()
                .ForMember(dest => dest.ReservationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => src.Customer.FirstName + " " + src.Customer.LastName))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));

            CreateMap<ExtraExpenseDto, ExtraExpenseModel>()
     .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice))
     .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.ExpenseDate ?? DateTime.MinValue));


            // 📥 RegisterRequestModel → User
            CreateMap<RegisterViewModel, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)); // Email → UserName eşlemesi

            // 📥 RegisterRequestModel → UserProfile
            CreateMap<RegisterViewModel, UserProfile>()
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.IdentityNumber, opt => opt.MapFrom(src => src.IdentityNumber));




            CreateMap<RoomMaintenanceAssignmentCreateRequestModel, RoomMaintenanceAssignmentDto>();











            CreateMap<ComplaintLogDto, ComplaintLogResponseModel>();
            CreateMap<ComplaintReplyRequestModel, ComplaintLogDto>().ReverseMap();

            CreateMap<CreateReservationRequestModel, CustomerDto>()
     .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
     .ForMember(dest => dest.LoyaltyPoints, opt => opt.MapFrom(src => 0))
     .ForMember(dest => dest.IsIdentityVerified, opt => opt.MapFrom(src => true))
     .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => (int?)null)) // User ilişkisi yoksa null
     .ForMember(dest => dest.BillingDetails, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.BillingDetails) ? "Standart Bireysel Fatura" : src.BillingDetails))
     .ForMember(dest => dest.NeedsIdentityCheck, opt => opt.MapFrom(src => false))
     .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
     .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
     .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
     .ForMember(dest => dest.IdentityNumber, opt => opt.MapFrom(src => src.IdentityNumber));


            CreateMap<CreateReservationRequestModel, KimlikBilgisiDto>();
           
            //CreateMap<RoomDto, RoomResponseModel>()
            //    .ForMember(dest => dest.ImagePath, opt =>
            //        opt.MapFrom(src => src.RoomImages.FirstOrDefault(x => x.IsMain).ImagePath))

            //    .ForMember(dest => dest.ImageGallery, opt =>
            //        opt.MapFrom(src => src.RoomImages.Select(x => x.ImagePath).ToList()));





            CreateMap<RoomImage, RoomImageDto>().ReverseMap();

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
      .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)); // ✅ EKLENDİ





            CreateMap<RoomDto, RoomListVm>()
                .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.ToString()))
                .ForMember(dest => dest.RoomStatusName, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePerNight))
                .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.FloorNumber.ToString()))
                .ReverseMap();


            // Request - DTO eşleşmeleri
            CreateMap<CreateReservationRequestModel, ReservationDto>()
        .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CheckIn))
        .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.CheckOut));


            // RoomDetailVm için DTO'dan ViewModel'e eşleşme
            CreateMap<RoomDto, RoomDetailVm>()
                .ForMember(dest => dest.RoomStatus, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePerNight))
                .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.FloorNumber.ToString()))
                .ReverseMap();

            CreateMap<RegisterViewModel, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<RegisterViewModel, UserProfile>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));


            CreateMap<EmployeeDto, EmployeeResponseModel>()
       .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
       .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
       .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
       .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.ToString()))
       .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.ToString()))
       .ForMember(dest => dest.SalaryType, opt => opt.MapFrom(src => src.SalaryType.ToString()))
       .ForMember(dest => dest.ShiftInfo, opt => opt.MapFrom(src => $"{src.ShiftStart:hh\\:mm} - {src.ShiftEnd:hh\\:mm}"))
       .ForMember(dest => dest.ShiftTime, opt => opt.MapFrom(src => $"{src.ShiftStart:hh\\:mm} - {src.ShiftEnd:hh\\:mm}"))
       .ForMember(dest => dest.HireDate, opt => opt.MapFrom(src =>
           src.HireDate.HasValue ? src.HireDate.Value.ToString("dd.MM.yyyy") : "-"))
       .ForMember(dest => dest.HourlyWageFormatted, opt => opt.MapFrom(src =>
           src.SalaryType == SalaryType.Hourly ? $"{src.HourlyWage} ₺/saat" : "-"))
       .ForMember(dest => dest.HourlyWage, opt => opt.MapFrom(src => (decimal?)src.HourlyWage))
       .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
       .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
       .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
       .ForMember(dest => dest.IdentityNumber, opt => opt.MapFrom(src => src.IdentityNumber))
       .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
       .ForMember(dest => dest.WeeklyOffDay, opt => opt.MapFrom(src => src.WeeklyOffDay))
      .ForMember(dest => dest.DayOff, opt => opt.MapFrom(src =>
       string.IsNullOrEmpty(src.WeeklyOffDay)
        ? "-"
        : src.WeeklyOffDay
        ));

            CreateMap<EmployeeDto, EmployeeDetailVm>()
    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
    .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.ToString()))
    .ForMember(dest => dest.SalaryType, opt => opt.MapFrom(src => src.SalaryType.ToString()))
    .ForMember(dest => dest.HourlyWage, opt => opt.MapFrom(src => src.HourlyWage))
    .ForMember(dest => dest.FixedSalary, opt => opt.MapFrom(src => src.MonthlySalary))
    .ForMember(dest => dest.ShiftStartTime, opt => opt.MapFrom(src => src.ShiftStart))
    .ForMember(dest => dest.ShiftEndTime, opt => opt.MapFrom(src => src.ShiftEnd))
    .ForMember(dest => dest.WeeklyOffDay, opt => opt.MapFrom(src => src.WeeklyOffDay))
    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<EmployeeDto, UpdateEmployeeRequest>()
      .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
      .ForMember(dest => dest.DayOff, opt => opt.MapFrom(src => src.WeeklyOffDay))
      .ForMember(dest => dest.ShiftStart, opt => opt.MapFrom(src => src.ShiftStart.ToString(@"hh\:mm")))
      .ForMember(dest => dest.ShiftEnd, opt => opt.MapFrom(src => src.ShiftEnd.ToString(@"hh\:mm")))
      .ForMember(dest => dest.MonthlySalary, opt => opt.MapFrom(src => src.MonthlySalary)) // ✅ Bunu ekle
      .ReverseMap()
      .AfterMap((src, dest) =>
      {
          dest.Position = Enum.TryParse<EmployeePosition>(src.Position, out var pos)
              ? pos
              : EmployeePosition.Receptionist;

          dest.SalaryType = Enum.TryParse<SalaryType>(src.SalaryType, out var sal)
              ? sal
              : SalaryType.Monthly;

          dest.ShiftStart = TimeSpan.TryParse(src.ShiftStart, out var sStart) ? sStart : TimeSpan.Zero;
          dest.ShiftEnd = TimeSpan.TryParse(src.ShiftEnd, out var sEnd) ? sEnd : TimeSpan.Zero;
          dest.PhoneNumber = src.PhoneNumber;
          dest.WeeklyOffDay = src.DayOff;
          dest.MonthlySalary = src.MonthlySalary; // ✅ Bu da burada olsun
      });

            CreateMap<EmployeeCreateVm, EmployeeDto>()
      .ForMember(dest => dest.Position, opt => opt.MapFrom(src => Enum.Parse<EmployeePosition>(src.PositionName)))
      .ForMember(dest => dest.SalaryType, opt => opt.MapFrom(src => Enum.Parse<SalaryType>(src.SalaryType)))
      .ForMember(dest => dest.WeeklyOffDay, opt => opt.MapFrom(src => src.WeeklyOffDay));


            // VARDİYA
            CreateMap<EmployeeShiftDto, EmployeeShiftResponseVm>()
      .ForMember(dest => dest.ShiftHours,
          opt => opt.MapFrom(src => $"{src.ShiftStart:hh\\:mm} - {src.ShiftEnd:hh\\:mm}"))
      .ForMember(dest => dest.AssignedEmployeeNames,
          opt => opt.MapFrom(src =>
              src.AssignedEmployees != null && src.AssignedEmployees.Any()
                  ? string.Join(", ", src.AssignedEmployees
                      .Where(e => e.Employee != null)
                      .Select(e => e.Employee.FirstName + " " + e.Employee.LastName))
                  : "Atama Yok"))
      .ForMember(dest => dest.AssignedEmployeesFullNames,
          opt => opt.MapFrom(src =>
              src.AssignedEmployees != null
                  ? src.AssignedEmployees
                      .Where(e => e.Employee != null)
                      .Select(e => e.Employee.FirstName + " " + e.Employee.LastName)
                      .ToList()
                  : new List<string>()));

            CreateMap<EmployeeShiftCreateVm, EmployeeShiftDto>()
      .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
      .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<EmployeeShiftDto, EmployeeShiftUpdateVm>()
       .ForMember(dest => dest.ShiftDate, opt => opt.MapFrom(src => src.ShiftDate))
       .ForMember(dest => dest.ShiftStart, opt => opt.MapFrom(src => src.ShiftStart))
       .ForMember(dest => dest.ShiftEnd, opt => opt.MapFrom(src => src.ShiftEnd))
       .ForMember(dest => dest.OvertimePay, opt => opt.MapFrom(src => src.OvertimePay))
       .ForMember(dest => dest.HasOvertime, opt => opt.MapFrom(src => src.HasOvertime))
       .ForMember(dest => dest.IsDayOff, opt => opt.MapFrom(src => src.IsDayOff))
       .ReverseMap();

            // VARDİYA ATAMASI
            CreateMap<EmployeeShiftAssignmentDto, EmployeeShiftAssignmentResponseVm>()
                .ForMember(dest => dest.EmployeeFullName,
                    opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
                .ForMember(dest => dest.ShiftType,
                    opt => opt.MapFrom(src => src.EmployeeShift.ShiftType))
                .ForMember(dest => dest.ShiftHours,
                    opt => opt.MapFrom(src =>
                        $"{src.EmployeeShift.ShiftStart.ToString(@"hh\:mm")} - {src.EmployeeShift.ShiftEnd.ToString(@"hh\:mm")}"))
                .ForMember(dest => dest.ShiftStatus,
                    opt => opt.MapFrom(src => src.ShiftStatus.ToString()));

            CreateMap<EmployeeShiftAssignmentCreateVm, EmployeeShiftAssignmentDto>()
         .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
         .ReverseMap();

            CreateMap<EmployeeShiftAssignmentDto, EmployeeShiftOvertimeListVm>()
      .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Employee.FirstName))
      .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Employee.LastName))
      .ForMember(dest => dest.AssignedDate, opt => opt.MapFrom(src => src.AssignedDate))
      .ForMember(dest => dest.ShiftTime, opt => opt.MapFrom(src =>
          $"{src.EmployeeShift.ShiftStart:hh\\:mm} - {src.EmployeeShift.ShiftEnd:hh\\:mm}"))
      .ForMember(dest => dest.WorkedHours, opt => opt.MapFrom(src =>
          (src.EmployeeShift.ShiftEnd - src.EmployeeShift.ShiftStart).TotalHours))
      .ForMember(dest => dest.OvertimeHours, opt => opt.MapFrom(src =>
          (src.EmployeeShift.ShiftEnd - src.EmployeeShift.ShiftStart).TotalHours > 8
              ? (src.EmployeeShift.ShiftEnd - src.EmployeeShift.ShiftStart).TotalHours - 8
              : 0));


            // RequestModel → Entity
            CreateMap<CreateRoomTypePriceRequestModel, RoomTypePrice>();
            // Entity → ResponseModel
            CreateMap<RoomTypePrice, RoomTypePriceResponseModel>()
                .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.ToString())); // Enum'ı string'e dönüştürüyoruz

            // RoomTypePriceDto → ResponseModel
            CreateMap<RoomTypePriceDto, RoomTypePriceResponseModel>()
                .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.RoomType.ToString()));

            CreateMap<CreateRoomTypePriceRequestModel, RoomTypePriceDto>().ReverseMap();

            CreateMap<CampaignDto, CampaignRequestModel>().ReverseMap();
            CreateMap<CampaignDto, CampaignResponseModel>().ReverseMap();

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
                .ForMember(dest => dest.ImageGallery, opt => opt.MapFrom(src =>
                    src.RoomImages.Select(x => x.ImagePath).ToList()))
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore())
                .ForMember(dest => dest.CleaningInfo, opt => opt.Ignore()) // ✅ BURAYI EKLE
                .AfterMap((src, dest) =>
                {
                    dest.ImagePath = src.RoomImages.FirstOrDefault(x => x.IsMain)?.ImagePath;
                });

            CreateMap<RoomDto, RoomUpdateVm>().ReverseMap();

            CreateMap<ReservationDto, ReservationAdminResponseModel>()
       .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => src.Customer.FirstName + " " + src.Customer.LastName))
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
       .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationDate));
        }
        



    

    }

}

