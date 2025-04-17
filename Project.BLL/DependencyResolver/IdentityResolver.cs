using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Project.Dal.ContextClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DependencyResolver
{
    public static class IdentityResolver
    {
        public static void AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<int>>(x =>
            {
                x.Password.RequireDigit = false;
                x.Password.RequiredLength = 3;
                x.Password.RequireLowercase = false;
                x.Password.RequireUppercase = false;
                x.SignIn.RequireConfirmedEmail = true;
                x.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<MyContext>()
          .AddDefaultTokenProviders();
            // .AddRoles<IdentityRole<int>>();// 💥 BU SATIRI EKLE

            // Not: AddDefaultTokenProviders eklenmezse şifre sıfırlama, mail onayı gibi işlemler çalışmaz
        }
    }
}
