using ChoreographySaga.PaymentService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChoreographySaga.PaymentService.Persistence;

public class PaymentDbContext(DbContextOptions<PaymentDbContext> options) : DbContext(options)
{
    public DbSet<User> Clients { get; set; }
}