using System.ComponentModel.DataAnnotations;

namespace ChoreographySaga.StocksService.Persistence.Entities;

public class ReservedProduct
{
    [Key]
    public Guid ReservationUuid { get; set; }
    public required decimal Quantity { get; set; }
    public required Guid ProductUuid { get; set; }
    public required Guid OrderUuid { get; set; }
}