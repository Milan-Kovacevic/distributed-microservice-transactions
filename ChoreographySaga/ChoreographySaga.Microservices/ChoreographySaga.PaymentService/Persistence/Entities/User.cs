using System.ComponentModel.DataAnnotations;

namespace ChoreographySaga.PaymentService.Persistence.Entities;

public class User
{
    [Key]
    public Guid UserUuid { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required decimal Balance { get; set; }
}