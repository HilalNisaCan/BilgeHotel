using Microsoft.Extensions.DependencyInjection;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DependencyResolver
{
    public static class RepositoryResolver
    {
        public static void AddRepositoryService(this IServiceCollection services)
        {

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
            services.AddScoped<IEmployeeShiftAssignmentRepository, EmployeeShiftAssignmentRepository >();
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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IRoomTypePriceRepository, RoomTypePriceRepository>();
            services.AddScoped<IRepository<Employee>, EmployeeRepository>(); 
            
     
        }
    }
}
