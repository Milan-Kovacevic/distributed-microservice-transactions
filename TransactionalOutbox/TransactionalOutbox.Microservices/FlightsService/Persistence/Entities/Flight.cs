using System.ComponentModel.DataAnnotations;

namespace FlightsService.Persistence.Entities;

public class Flight
{
    [Key]
    public Guid FlightUuid { get; set; }
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public decimal Price { get; set; }
}