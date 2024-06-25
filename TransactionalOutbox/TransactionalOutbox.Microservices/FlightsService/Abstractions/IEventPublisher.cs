using FlightsService.Persistence.Entities;
using TransactionalOutbox.Contracts.Events;

namespace FlightsService.Abstractions;

public interface IEventPublisher
{
    public Task PublishEvent(OutboxEntity request);
}