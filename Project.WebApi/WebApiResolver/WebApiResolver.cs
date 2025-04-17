using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.BLL.Services.abstracts;
using Project.BLL.Services.Concretes;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.Entities.Models;

namespace Project.WebApi.WebApiResolver
{
    public static class WebApiResolver
    {
        public static void AddWebApiResolvers(this IServiceCollection services)
        {

            // REPOSITORYLER
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IBackUpRepository, BackupLogRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<IComplaintLogRepository, ComplaintLogRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IEarlyReservationDiscountRepository, EarlyReservationDiscountRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeShiftRepository, EmployeeShiftRepository>();
            services.AddScoped<IEmployeeShiftAssignmentRepository, EmployeeShiftAssignmentRepository>();
            services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddScoped<IExtraExpenseRepository, ExtraExpenseRepository>();
            services.AddScoped<IGuestVisitLogRepository, GuestVisitLogRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IReportLogRepository, ReportLogRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomCleaningScheduleRepository, RoomCleaningScheduleRepository>();
            services.AddScoped<IRoomImageRepository, RoomImageRepository>();
            services.AddScoped<IRoomMaintenanceAssignmentRepository, RoomMaintenanceAssignmentRepository>();
            services.AddScoped<IRoomMaintenanceRepository, RoomMaintenanceRepository>();
            services.AddScoped<IRoomTypePriceRepository, RoomTypePriceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIdentityValidationService, TCKimlikDogrulamaService>(); // Eğer bu senin kendi servissen





            // MANAGERLAR
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<IEmployeeManager, EmployeeManager>();
            services.AddScoped<IEmployeeShiftAssignmentManager, EmployeeShiftAssignmentManager>();
            services.AddScoped<IEmployeeShiftManager, EmployeeShiftManager>();
            services.AddScoped<IRoomManager, RoomManager>();
            services.AddScoped<IReservationManager, ReservationManager>();
            services.AddScoped<IEarlyReservationDiscountManager, EarlyReservationDiscountManager>();
            services.AddScoped<ICustomerManager, CustomerManager>();
            services.AddScoped<IRoomTypePriceManager, RoomTypePriceManager>();
            services.AddScoped<ICampainManager, CampaignManager>();
            services.AddScoped<IPaymentManager, PaymentManager>();
            services.AddScoped<IExtraExpenseManager, ExtraExpenseManager>();
            services.AddScoped<IComplaintLogManager, ComplaintLogManager>();
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();    



           
        }
    }
}
