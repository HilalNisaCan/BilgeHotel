using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Project.BLL.Services.abstracts;
using Project.BLL.Services.Concretes;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DependencyResolver
{
    public static class ServiceResolver
    {
        public static void AddServiceDependencies(this IServiceCollection services)
        {
            //services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IIdentityValidationService, TCKimlikDogrulamaService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();



        }
    }
}
