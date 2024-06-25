using ChoreographySaga.OrdersService.Abstractions;
using ChoreographySaga.OrdersService.Persistence;
using ChoreographySaga.OrdersService.Services;
using MassTransit;
using MySql.EntityFrameworkCore.Extensions;

namespace ChoreographySaga.OrdersService.Configuration;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Adding the MySQL Database context
        services.AddMySQLServer<OrdersDbContext>(configuration.GetConnectionString("Database")!);
        // Adding the Order service
        services.AddScoped<IOrderService, OrderService>();
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