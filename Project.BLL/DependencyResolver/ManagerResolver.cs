using AutoMapper;
using Castle.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.BLL.Managers.Abstracts;
using Project.BLL.Managers.Concretes;
using Project.BLL.Services.abstracts;
using Project.BLL.Services.Concretes;
using Project.Dal.Repositories.Abstracts;
using Project.Dal.Repositories.Concretes;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Project.BLL.DependencyResolver
{
    public static class ManagerResolver
    {
        public static void AddManagerService(this IServiceCollection services)
        {
            services.AddScoped<IUserManager,UserManager>();
       
            services.AddScoped<IOrderManager, OrderManager>();

            services.AddScoped<IEmployeeManager, EmployeeManager>();
            services.AddScoped<IEmployeeShiftAssignmentManager, EmployeeShiftAssignmentManager>();
            services.AddScoped<IEmployeeShiftManager, EmployeeShiftManager>();
            services.AddScoped<IRoomManager, RoomManager>();
            services.AddScoped<IReservationManager, ReservationManager>();
            services.AddScoped<IEarlyReservationDiscountManager, EarlyReservationDiscountManager>();
            services.AddScoped<ICustomerManager, CustomerManager>();
            services.AddScoped<IRoomTypePriceManager, RoomTypePriceManager>();
            services.AddScoped<IReportLogManager, ReportLogManager>();
            services.AddScoped<ICampainManager,CampaignManager>();
            services.AddScoped<IPaymentManager,PaymentManager>();
            services.AddScoped<IExtraExpenseManager, ExtraExpenseManager>();
            services.AddScoped<IComplaintLogManager, ComplaintLogManager>();
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<IRoomCleaningScheduleManager, RoomCleaningScheduleManager>();
            services.AddScoped<IRoomMaintenanceAssignmentManager,RoomMaintenanceAssignmentManager>();
            services.AddScoped<IRoomMaintenanceManager, RoomMaintenanceManager>();
            services.AddScoped<IGuestVisitLogManager,GuestVisitLogManager>();
            services.AddScoped<IReviewManager, ReviewManager>();
            //services.AddScoped<IBackupLogManager>(provider =>
            //{
            //    var repository = provider.GetRequiredService<IBackUpRepository>();
            //    var userRepo = provider.GetRequiredService<IUserRepository>();
            //    var mapper = provider.GetRequiredService<IMapper>();
            //    var config = provider.GetRequiredService<Microsoft.Extensions.Configuration.IConfiguration>();

            //    string connStr = config.GetConnectionString("MyConnection");

            //    return new BackupLogManager(repository, userRepo, connStr, mapper);
            //});





        }
    }
}
