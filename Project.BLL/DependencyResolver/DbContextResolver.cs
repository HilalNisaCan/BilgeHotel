﻿using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project.Dal.ContextClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project.BLL.DependencyResolver
{
    public static class DbContextResolver
    {
        public static void AddDbContextService(this IServiceCollection services)
        {
            //ServiceProvider size bir hizmet saglayıcısı nesnesi sunarak istediginiz ayarlama dosyasına erişmenizin temelini atar

            ServiceProvider provider = services.BuildServiceProvider();

            IConfiguration configuration = provider.GetRequiredService<IConfiguration>();

            services.AddDbContext<MyContext>(x =>
                x.UseSqlServer(configuration.GetConnectionString("MyConnection"))
                 .UseLazyLoadingProxies());
        }
    }
}
