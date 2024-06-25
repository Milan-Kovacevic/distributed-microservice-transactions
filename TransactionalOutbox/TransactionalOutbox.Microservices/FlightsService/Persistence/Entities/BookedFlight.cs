using System.ComponentModel.DataAnnotations;

namespace FlightsService.Persistence.Entities;

public class BookedFlight : OutboxEntity
{
    [Key]
    public Guid BookingUuid { get; set; }
    public Guid FlightUuid { get; set; }
    public required Flight Flight { get; set; }
    public int NumberOfTickets { get; set; }
    public decimal TotalPrice { get; set; }
}