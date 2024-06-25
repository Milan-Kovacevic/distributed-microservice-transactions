using ChoreographySaga.StocksService.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ChoreographySaga.StocksService.Configuration;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Adding the Postgresql Database context
        services.AddDbContext<StocksDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));
        // Adding the RabbitMQ Broker
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            // Adding all event consumers of interest
            // (event consumers from current assembly that implements IConsumer will be registered in DI container)
            // (... by reflection behind the hood)
            busConfigurator.AddConsumers(typeof(ConfigureServices).Assembly);
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                {
                    h.Username(configuration["MessageBroker:Username"]!);
                    h.Password(configuration["MessageBroker:Password"]!);
                });

                configurator.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}