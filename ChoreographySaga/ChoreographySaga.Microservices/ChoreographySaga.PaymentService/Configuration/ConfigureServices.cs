using ChoreographySaga.PaymentService.Persistence;
using ChoreographySaga.PaymentService.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ChoreographySaga.PaymentService.Configuration;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Adding the Postgresql Database context
        services.AddDbContext<PaymentDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));
        // Adding the Payment Service
        services.AddScoped<IPaymentService, Services.PaymentService>();
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