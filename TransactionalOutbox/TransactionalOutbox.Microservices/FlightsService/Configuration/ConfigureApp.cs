using FlightsService.Persistence;
using FlightsService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightsService.Configuration;

public static class ConfigureApp
{
    public static IApplicationBuilder ApplyMigration(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FlightsDbContext>();
        dbContext.Database.Migrate();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<FlightsDbContext>>();
        logger.LogInformation("Performed Database Migration on {TimeUtc}", DateTime.UtcNow);
        return app;
    }

    public static void ApplyDataSeed(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FlightsDbContext>();
        if (dbContext.Flights.Any())
            return;

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<FlightsDbContext>>();

        dbContext.Flights.AddRange([
            new Flight
            {
                FlightUuid = Guid.Parse("109fd407-e422-40ca-bf03-db02ba91eea7"),
                DepartureAirport = "Belgrade Nikola Tesla (BEG)",
                ArrivalAirport = "Frankfurt am Main (FRA)",
                DepartureTime = new DateTime(2024, 6, 28, 19, 5, 0, DateTimeKind.Utc),
                ArrivalTime = new DateTime(2024, 6, 28, 21, 5, 0, DateTimeKind.Utc),
                Price = 249.99m
            },
            new Flight
            {
                FlightUuid = Guid.Parse("91891e25-7bde-4475-ba13-5f8d7fd52695"),
                DepartureAirport = "Istanbul (IST)",
                ArrivalAirport = "New York John F Kennedy Intl (JFK)",
                DepartureTime = new DateTime(2024, 7, 4, 18, 45, 0, DateTimeKind.Utc),
                ArrivalTime = new DateTime(2024, 6, 28, 22, 30, 0, DateTimeKind.Utc),
                Price = 399.99m
            },
        ]);
        dbContext.SaveChanges();
        logger.LogInformation("Applied seeding of the database at {Time}", DateTime.UtcNow);
    }
}