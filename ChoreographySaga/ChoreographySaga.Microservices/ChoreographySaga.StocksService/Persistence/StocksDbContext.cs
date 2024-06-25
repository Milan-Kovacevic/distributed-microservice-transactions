using ChoreographySaga.StocksService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChoreographySaga.StocksService.Persistence;

public class StocksDbContext(DbContextOptions<StocksDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ReservedProduct> ReservedProducts { get; set; }
}