using AutoMapper;
using Project.BLL.DtoClasses;
using Project.MvcUI.Areas.Admin.Models.PureVm.RequestModel.ExcahangeRate;
using Project.MvcUI.Areas.Admin.Models.PureVm.ResponseModel.ExcahangeRate;

namespace Project.MvcUI.VmMapping
{
    /// <summary>
    /// ExchangeRate ViewModel ↔ DTO dönüşümlerini tanımlar
    /// </summary>
    public class ExchangeRateProfile : Profile
    {
        public ExchangeRateProfile()
        {
            // 🎯 Create & Update için: RequestModel → DTO
            CreateMap<ExchangeRateRequestModel, ExchangeRateDto>()
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.FromCurrency, opt => opt.MapFrom(src => src.FromCurrency))
                .ForMember(dest => dest.ToCurrency, opt => opt.MapFrom(src => src.ToCurrency))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
                .ForMember(dest => dest.Date, opt =>
                {
                    opt.PreCondition(src => src.Date == default);         // sadece default ise çalış
                    opt.MapFrom(_ => DateTime.Now);                       // default ise şimdi atanır
                });

            // 📝 Güncelleme formuna veri göstermek için: DTO → RequestModel
            CreateMap<ExchangeRateDto, ExchangeRateRequestModel>()
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.FromCurrency, opt => opt.MapFrom(src => src.FromCurrency))
                .ForMember(dest => dest.ToCurrency, opt => opt.MapFrom(src => src.ToCurrency))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date)); // burada tarihi göster

            // 📋 DTO'dan ResponseModel'e dönüşüm (listeleme ve detay için)
            CreateMap<ExchangeRateDto, ExchangeRateResponseModel>()
                .ForMember(dest => dest.FromCurrency, opt => opt.MapFrom(src => src.FromCurrency.ToString()))
                .ForMember(dest => dest.ToCurrency, opt => opt.MapFrom(src => src.ToCurrency.ToString()))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));
        }
    }
}
