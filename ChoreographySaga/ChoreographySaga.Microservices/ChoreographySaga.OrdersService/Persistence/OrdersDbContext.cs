using ChoreographySaga.OrdersService.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChoreographySaga.OrdersService.Persistence;

public class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("orders");
        
        var orderModel = modelBuilder.Entity<Order>();
        orderModel.HasKey(o => o.OrderUuid);
        orderModel.HasIndex(o => o.OrderUuid).IsUnique();
        orderModel.Property(o => o.OrderDate).IsRequired();
        orderModel.Property(o => o.TotalPrice).IsRequired();
        orderModel.Property(o => o.Status).IsRequired();
        orderModel.Property(o => o.CustomerUuid).IsRequired();
        orderModel.HasMany<OrderProduct>(o => o.Products).WithOne();
        
        var orderedProductsModel = modelBuilder.Entity<OrderProduct>();
        orderedProductsModel.HasKey(x => x.OrderProductUuid);
        orderedProductsModel.HasIndex(x => x.OrderProductUuid).IsUnique();
        orderedProductsModel.Property(x => x.ProductUuid).IsRequired();
        orderedProductsModel.Property(x => x.Price).IsRequired();
        orderedProductsModel.Property(x => x.Quantity).IsRequired();
        base.OnModelCreating(modelBuilder);
    }
}