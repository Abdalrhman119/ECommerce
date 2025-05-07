using Microsoft.Extensions.DependencyInjection;
using Services.Mapping;
using ServicesAbstraction;

namespace Services
{
    public static class ApplicationServiceRegisteration
    {

        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddAutoMapper(typeof(ProductProfile).Assembly);

            return services;
        }
    }
}
