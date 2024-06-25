using System.ComponentModel.DataAnnotations;

namespace TwoPhaseCommit.Participant.Persistence.Entities;

public class TransactionEntity
{
    [Key]
    public Guid Uuid { get; set; }    
    public required string Data { get; set; }    
    public required Guid TransactionId { get; set; } 
}