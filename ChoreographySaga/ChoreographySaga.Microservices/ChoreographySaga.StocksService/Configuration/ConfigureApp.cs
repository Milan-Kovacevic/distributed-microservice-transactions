using ChoreographySaga.StocksService.Persistence;
using ChoreographySaga.StocksService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChoreographySaga.StocksService.Configuration;

public static class ConfigureApp
{
    public static IApplicationBuilder ApplyMigration(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StocksDbContext>();
        dbContext.Database.Migrate();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<StocksDbContext>>();
        logger.LogInformation("Performed Database Migration on {TimeUtc}", DateTime.UtcNow);
        return app;
    }
    
    public static void ApplyDataSeed(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<StocksDbContext>();
        if (dbContext.Products.Any())
            return;
        
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<StocksDbContext>>();
        
        dbContext.Products.AddRange([
            new Product()
            {
                ProductUuid = Guid.Parse("1d7dd173-8133-4e7a-8042-05c9c13147be"),
                Name = "First Product Seed",
                AvailableQuantity = 100m,
                UnitPrice = 19.99m
            },
            new Product()
            {
                ProductUuid = Guid.Parse("001c6b26-6bc0-46b7-ac61-078e38941539"),
                Name = "Second Product Seed",
                AvailableQuantity = 500m,
                UnitPrice = 14.49m
            },
            new Product()
            {
                ProductUuid = Guid.Parse("ce27f1a2-1aa0-4acb-b941-8ab08e49458f"),
                Name = "Third Product Seed",
                AvailableQuantity = 1000m,
                UnitPrice = 9.98m
            },
        ]);
        dbContext.SaveChanges();
        logger.LogInformation("Applied seeding of the database at {Time}", DateTime.UtcNow);
    }
}