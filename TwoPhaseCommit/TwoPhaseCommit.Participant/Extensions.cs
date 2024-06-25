using Microsoft.EntityFrameworkCore;
using TwoPhaseCommit.Participant.Persistence;

namespace TwoPhaseCommit.Participant;

public static class Extensions
{
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
        logger.LogInformation("Applied migration for the database context at {Time}", DateTime.UtcNow);
        return app;
    }
}