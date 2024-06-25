using Microsoft.EntityFrameworkCore;
using TwoPhaseCommit.Participant.Persistence.Entities;

namespace TwoPhaseCommit.Participant.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<TransactionLog> Logs { get; set; }
}