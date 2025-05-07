
using AutoMapper;
using Domain.Contracts;
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

namespace ECommerce_Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddInfrastructureRegisteration(builder.Configuration);
            builder.Services.AddAplicationServices();
            builder.Services.AddWebApplicationServices();


            //builder.Services.AddScoped<PictureUrlResolver>();


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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
