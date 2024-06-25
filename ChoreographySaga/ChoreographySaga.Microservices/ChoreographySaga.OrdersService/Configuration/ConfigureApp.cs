using ChoreographySaga.OrdersService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChoreographySaga.OrdersService.Configuration;

public static class ConfigureApp
{
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
        dbContext.Database.Migrate();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrdersDbContext>>();
        logger.LogInformation("Performed Database Migration on {TimeUtc}", DateTime.UtcNow);
        return app;
    }
}