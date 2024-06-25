using System.Collections.Immutable;
using FlightsService.Abstractions;
using FlightsService.BackgroundJobs;
using FlightsService.EventPublishers;
using FlightsService.Persistence;
using FlightsService.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace FlightsService.Configuration;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Adding the Postgresql Database context
        services.AddDbContext<FlightsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));
        // Adding the Payment Service
        services.AddScoped<IFlightService, FlightService>();
        // Adding the RabbitMQ Broker
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
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
        // Adding the Event Publishers
        services.AddScoped<IEventPublisher, FlightBookedEventPublisher>();
        // Adding Quartz background service
        services.AddQuartz(configure =>
        {
            // Will use default microsoft DI Container Job Factory for injection background job implementation
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
            configure.AddJob<ProcessOutboxMessagesJob>(jobKey).AddTrigger(trigger =>
                trigger.ForJob(jobKey)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(30).RepeatForever()));
        });
        services.AddQuartzHostedService();
        
        return services;
    }
}