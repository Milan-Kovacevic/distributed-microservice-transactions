using ChoreographySaga.PaymentService.Persistence;
using ChoreographySaga.PaymentService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChoreographySaga.PaymentService.Configuration;

public static class ConfigureApp
{
    public static IApplicationBuilder ApplyMigration(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
        dbContext.Database.Migrate();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<PaymentDbContext>>();
        logger.LogInformation("Performed Database Migration on {TimeUtc}", DateTime.UtcNow);
        return app;
    }
    
    public static void ApplyDataSeed(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
        if (dbContext.Clients.Any())
            return;
        
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<PaymentDbContext>>();
        
        dbContext.Clients.AddRange([
            new User()
            {
                UserUuid = Guid.Parse("175c85ae-854b-45d1-a9a0-65e065c71a93"),
                FirstName = "Marko",
                LastName = "Markovic",
                Balance = 500.00m
            },
            new User()
            {
                UserUuid = Guid.Parse("a9eada73-4027-4362-968c-bec28a87699f"),
                FirstName = "Janko",
                LastName = "Jankovic",
                Balance = 250.00m
            },
        ]);
        dbContext.SaveChanges();
        logger.LogInformation("Applied seeding of the database at {Time}", DateTime.UtcNow);
    }
}