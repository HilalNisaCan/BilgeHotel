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
            services.AddScoped<ICampainManager,CampaignManager>();
            services.AddScoped<IPaymentManager,PaymentManager>();
            services.AddScoped<IExtraExpenseManager, ExtraExpenseManager>();
            services.AddScoped<IComplaintLogManager, ComplaintLogManager>();
            services.AddScoped<IProductManager, ProductManager>();

         



        }
    }
}
