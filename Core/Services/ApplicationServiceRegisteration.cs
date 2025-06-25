using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Mapping;
using ServicesAbstraction;
using Shared.Authentication;

namespace Services
{
    public static class ApplicationServiceRegisteration
    {

        public static IServiceCollection AddAplicationServices(this IServiceCollection services, 
                                                                        IConfiguration configuration)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddAutoMapper(typeof(ProductProfile).Assembly);
            services.Configure<JWTOptions>(configuration.GetSection("JWTOptions"));


            return services;
        }
    }
}
