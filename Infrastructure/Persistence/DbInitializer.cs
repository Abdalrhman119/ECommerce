using Domain.Contracts;
using Domain.Models.Products;
using Persistence.Data;
using System.Text.Json;

namespace Persistence
{
    public class DbInitializer(StoreDbContext _storeDbContext) : IDbInitializer
    {
        public async Task InitializeAsync()
        {
            //In Deployment
            //if ((await _storeDbContext.Database.GetPendingMigrationsAsync()).Any())
            //{
            //    await _storeDbContext.Database.MigrateAsync();
            //}
            //In Development
            try
            {
                if (!_storeDbContext.Set<ProductBrand>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(data);
                    if (brands is not null && brands.Any())
                    {

                        _storeDbContext.Set<ProductBrand>().AddRange(brands);
                        await _storeDbContext.SaveChangesAsync();
                    }
                }
                if (!_storeDbContext.Set<ProductType>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");
                    var Types = JsonSerializer.Deserialize<List<ProductType>>(data);
                    if (Types is not null && Types.Any())
                    {

                        _storeDbContext.Set<ProductType>().AddRange(Types);
                        await _storeDbContext.SaveChangesAsync();
                    }
                }
                if (!_storeDbContext.Set<Product>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");
                    var Products = JsonSerializer.Deserialize<List<Product>>(data);
                    if (Products is not null && Products.Any())
                    {

                        _storeDbContext.Set<Product>().AddRange(Products);
                        await _storeDbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
