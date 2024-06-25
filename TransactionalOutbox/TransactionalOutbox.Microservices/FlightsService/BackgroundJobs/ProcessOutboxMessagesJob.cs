using FlightsService.Abstractions;
using FlightsService.Persistence;
using FlightsService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace FlightsService.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(
    FlightsDbContext dbContext,
    ILogger<ProcessOutboxMessagesJob> logger,
    IEventPublisher flightBookedEventPublisher) : IJob
{
    private readonly Dictionary<OutboxType, IEventPublisher> _eventPublishers = new()
    {
        { OutboxType.FlightBooked, flightBookedEventPublisher }
    };

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Starting execution of background job '{Name}'", nameof(ProcessOutboxMessagesJob));
        var outboxMessages = await dbContext.OutboxMessages
            .Where(x => x.ProcessedOn == null)
            .OrderBy(x => x.OccuredOn)
            .Take(5)
            .ToListAsync(context.CancellationToken);
        foreach (var outboxMessage in outboxMessages)
        {
            var entity = JsonConvert.DeserializeObject<OutboxEntity>(outboxMessage.ContentJson,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            if (entity is null)
            {
                logger.LogCritical("Unable to deserialize outbox message json content '{Content}'",
                    outboxMessage.ContentJson);
                continue;
            }

            // Obtain the event publisher for a given outbox message type
            var eventPublisher = _eventPublishers.GetValueOrDefault(outboxMessage.Type);
            if (eventPublisher is null)
            {
                logger.LogCritical("Event publisher for outbox message type '{Type}' was not found",
                    outboxMessage.Type);
                continue;
            }

            try
            {
                await eventPublisher.PublishEvent(entity);
            }
            catch (Exception ex)
            {
                // Catch any exceptions that may occur during event publishing here
                outboxMessage.Error = "Exception occured when trying to publish event";
                await dbContext.SaveChangesAsync();
                // TODO: Implement retry mechanisms or something else...
                return;
            }

            outboxMessage.ProcessedOn = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
        }
    }
}