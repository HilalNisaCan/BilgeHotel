using AutoMapper;
using Project.BLL.DtoClasses;
using Project.Entities.Enums;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.Employee;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.Employee;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.EmployeeShift;

namespace Project.MvcUI.VmMapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            // ------------------- 👨‍💼 EMPLOYEE -------------------

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

            CreateMap<EmployeeDto, EmployeeSalaryResultVm>()
       .ForMember(dest => dest.EmployeeFullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }

}
