﻿using Microsoft.Extensions.Configuration;
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
            services.AddAutoMapper(typeof(ProductProfile).Assembly);

            services.AddScoped<IServiceManager, ServiceManagerWithFactoryDelegate>();


            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IPaymentService, PaymentService>();



            services.AddScoped<Func<IProductService>>(provider => ()
                => provider.GetRequiredService<IProductService>());

            services.AddScoped<Func<IBasketService>>(provider => ()
                => provider.GetRequiredService<IBasketService>());

            services.AddScoped<Func<IOrderService>>(provider => ()
                => provider.GetRequiredService<IOrderService>());

            services.AddScoped<Func<IAuthenticationService>>(provider => ()
                => provider.GetRequiredService<IAuthenticationService>());

            services.AddScoped<Func<IPaymentService>>(provider => ()
                => provider.GetRequiredService<IPaymentService>());

            services.Configure<JWTOptions>(configuration.GetSection("JWTOptions"));

            return services;
        }

    }
}