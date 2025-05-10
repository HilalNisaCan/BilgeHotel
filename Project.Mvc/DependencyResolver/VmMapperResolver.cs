using Project.BLL.Mapping;
using Project.MvcUI.VmMapping;

namespace Project.MvcUI.DependencyResolver
{
    public static class VmMapperResolver
    {
        public static void AddVmMapperService(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<CampaignAndPricingProfile>();
                cfg.AddProfile<ComplaintAndExpenseProfile>();
                cfg.AddProfile<EmployeeProfile>();
                cfg.AddProfile<PaymentVmProfile>();
                cfg.AddProfile<ReservationProfile>();
                cfg.AddProfile<RoomProfile>();
                cfg.AddProfile<ShiftProfile>();
                cfg.AddProfile<UserVmProfile>();
                // Genel karma yapı (multimap profile)
                cfg.AddProfile<ExchangeRateProfile>();
            });

        }
    }
}
