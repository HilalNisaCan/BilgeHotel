using AutoMapper;
using Project.BLL.DtoClasses;
using Project.MvcUI.PaymentApiTools;

namespace Project.MvcUI.VmMapping
{
    public class PaymentVmProfile:Profile
    {
        public PaymentVmProfile()
        {
            // Web üzerinden gelen ödeme formu -> DTO
            CreateMap<PaymentRequestModel, PaymentDto>()
           .ForMember(dest => dest.Description, opt => opt.Ignore())
           .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
           .ForMember(dest => dest.LastUpdated, opt => opt.Ignore())
           .ForMember(dest => dest.PaymentDate, opt => opt.Ignore())
           .ForMember(dest => dest.ReservationId, opt => opt.Ignore())
           .ForMember(dest => dest.UserId, opt => opt.Ignore())
           .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
           .ForMember(dest => dest.InvoiceNumber, opt => opt.Ignore())
           .ForMember(dest => dest.TransactionId, opt => opt.Ignore());
        }
    }
}
