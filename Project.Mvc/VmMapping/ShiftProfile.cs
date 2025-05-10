using AutoMapper;
using Project.BLL.DtoClasses;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.EmployeeShift;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.EmployeeShift;

namespace Project.MvcUI.VmMapping
{

    public class ShiftProfile : Profile
    {
        public ShiftProfile()
        {
            // ------------------- 🕐 SHIFT -------------------

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

            // ------------------- 🔁 SHIFT ASSIGNMENT -------------------

            CreateMap<EmployeeShiftAssignmentDto, EmployeeShiftAssignmentResponseVm>()
                .ForMember(dest => dest.EmployeeFullName,
                    opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
                .ForMember(dest => dest.ShiftType,
                    opt => opt.MapFrom(src => src.EmployeeShift.ShiftType))
                .ForMember(dest => dest.ShiftHours,
                    opt => opt.MapFrom(src =>
                        $"{src.EmployeeShift.ShiftStart:hh\\:mm} - {src.EmployeeShift.ShiftEnd:hh\\:mm}"))
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
        }
    }

}
