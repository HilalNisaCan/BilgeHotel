using Project.BLL.Mapping;
using Project.MvcUI.VmMapping;

namespace Project.MvcUI.DependencyResolver
{
    public static class VmMapperResolver
    {
        public static void AddVmMapperService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(VmMappingProfile));
        }
    }
}
