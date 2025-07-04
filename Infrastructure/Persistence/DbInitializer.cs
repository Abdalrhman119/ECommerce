﻿using Domain.Contracts;
using Domain.Models.Identity;
using Domain.Models.Orders;
using Domain.Models.Products;
using Microsoft.AspNetCore.Identity;
using Persistence.Data;
using System.Text.Json;

namespace Persistence
{
    public class DbInitializer(StoreDbContext _storeDbContext,
                                UserManager<ApplicationUser> _userManager,
                                RoleManager<IdentityRole> _roleManager) : IDbInitializer
    {
        public async Task InitializeAsync()
        {
            //In Deployment
            //if((await _storeDbContext.Database.GetPendingMigrationsAsync()).Any())
            //{
            //    await _storeDbContext.Database.MigrateAsync();
            //}

            //In Development

            try
            {
                if (!_storeDbContext.Set<ProductBrand>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\infrastructure\Persistence\Data\Seeding\brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(data);
                    if (brands is not null && brands.Any())
                    {
                        _storeDbContext.Set<ProductBrand>().AddRange(brands);
                        await _storeDbContext.SaveChangesAsync();
                    }
                }
                if (!_storeDbContext.Set<ProductType>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\infrastructure\Persistence\Data\Seeding\types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(data);
                    if (types is not null && types.Any())
                    {
                        _storeDbContext.Set<ProductType>().AddRange(types);
                        await _storeDbContext.SaveChangesAsync();
                    }
                }
                if (!_storeDbContext.Set<Product>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\infrastructure\Persistence\Data\Seeding\products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(data);
                    if (products is not null && products.Any())
                    {
                        _storeDbContext.Set<Product>().AddRange(products);
                        await _storeDbContext.SaveChangesAsync();
                    }
                }
                if (!_storeDbContext.Set<DeliveryMethod>().Any()) 
                {
                    var data = await File.ReadAllTextAsync(@"..\infrastructure\Persistence\Data\Seeding\deliveryMethods.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(data);
                    if (deliveryMethods is not null && deliveryMethods.Any())
                    {
                        _storeDbContext.Set<DeliveryMethod>().AddRange(deliveryMethods);
                        await _storeDbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }

        public async Task InitializeIdentityAsync()
        {
            //In Deployment
            //if((await _storeDbContext.Database.GetPendingMigrationsAsync()).Any())
            //{
            //    await _storeDbContext.Database.MigrateAsync();
            //}

            //In Development

            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var superAdminUser = new ApplicationUser()
                    {
                        DisplayName = "Super Admin",
                        UserName = "superadmin",
                        Email = "superadmin@gmail.com"
                    };

                    var AdminUser = new ApplicationUser()
                    {
                        DisplayName = "Admin",
                        UserName = "admin",
                        Email = "admin@gmail.com"
                    };

                    await _userManager.CreateAsync(superAdminUser, "P@ssw0rd");
                    await _userManager.CreateAsync(AdminUser, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                    await _userManager.AddToRoleAsync(AdminUser, "Admin");
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
    }
}