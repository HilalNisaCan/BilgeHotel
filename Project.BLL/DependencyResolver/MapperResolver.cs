using Microsoft.Extensions.DependencyInjection;
using Project.BLL.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DependencyResolver
{
    public static class MapperResolver
    {
        public static void AddMapperService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
