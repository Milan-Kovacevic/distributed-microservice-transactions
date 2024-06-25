using FlightsService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlightsService.Persistence;

public class FlightsDbContext(DbContextOptions<FlightsDbContext> options) : DbContext(options)
{
    public DbSet<Flight> Flights { get; set; }
    public DbSet<BookedFlight> BookedFlights { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BookedFlight>()
            .HasOne(e => e.Flight)
            .WithMany()
            .HasForeignKey(x=> x.FlightUuid)
            .IsRequired();
    }
}