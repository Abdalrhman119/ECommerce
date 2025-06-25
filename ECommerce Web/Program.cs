using AutoMapper;
using Domain.Contracts;
using ECommerce.Web;
using ECommerce_Web.Factories;
using ECommerce_Web.Middelwares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Repositaries;
using Services;
using Services.Mapping;
using ServicesAbstraction;
using Shared.ErrorModels;
using Presentation.Controllers; // Ensure your controllers' namespace is referenced

namespace ECommerce_Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddInfrastructureRegisteration(builder.Configuration);
            builder.Services.AddAplicationServices(builder.Configuration);
            builder.Services.AddWebApplicationService(builder.Configuration);
            builder.Services.AddControllers() // Registers controllers for DI and routing
                .AddApplicationPart(typeof(AuthenticationController).Assembly); // Explicitly add the assembly containing your controllers
            builder.Services.AddEndpointsApiExplorer(); // Required for Swagger
            builder.Services.AddSwaggerGen(); // Registers Swagger generator

            var app = builder.Build();

            await app.InitializeDbAsync();

            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers(); // This enables attribute routing for controllers

            app.Run();
        }
    }
}
