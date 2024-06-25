using System.ComponentModel.DataAnnotations;

namespace TwoPhaseCommit.Participant.Persistence.Entities;

public class TransactionLog
{
    [Key]
    public Guid LogId { get; set; }    
    public required TransactionStatus Status { get; set; }    
    public required Guid TransactionId { get; set; }    
    public required string Data { get; set; }
}

public enum TransactionStatus {
    Prepared,
    Commited,
    Aborted,
}