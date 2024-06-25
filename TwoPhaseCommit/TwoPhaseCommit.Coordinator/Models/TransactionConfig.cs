using System.ComponentModel.DataAnnotations;

namespace TwoPhaseCommit.Coordinator.Models;

// Configuration for the distributed transaction example
public class TransactionConfig
{
    // Host that are involved in a transaction
    [Required] public List<string> Participants { get; set; } = [];
}