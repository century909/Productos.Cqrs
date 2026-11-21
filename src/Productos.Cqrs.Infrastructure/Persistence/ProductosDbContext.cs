using Microsoft.EntityFrameworkCore;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Infrastructure.Persistence;

public sealed class ProductosDbContext(DbContextOptions<ProductosDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductosDbContext).Assembly);
}
