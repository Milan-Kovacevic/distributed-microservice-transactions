using System.ComponentModel.DataAnnotations;

namespace FlightsService.Persistence.Entities;

public class OutboxMessage
{
    [Key]
    public Guid OutboxUuid { get; set; }
    public OutboxType Type { get; set; }
    public string ContentJson { get; set; }
    public DateTime OccuredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public string? Error { get; set; }
}

public enum OutboxType
{
    FlightBooked
}