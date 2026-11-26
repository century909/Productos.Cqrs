using Microsoft.EntityFrameworkCore;
using Npgsql;
using Productos.Cqrs.Application.Common;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Infrastructure.Persistence;

public sealed class ProductosDbContext(DbContextOptions<ProductosDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductosDbContext).Assembly);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (EsNombreDuplicado(ex))
        {
            throw new NombreProductoDuplicadoException(ex);
        }
    }

    private static bool EsNombreDuplicado(DbUpdateException ex)
        => ex.InnerException is PostgresException
        {
            SqlState: PostgresErrorCodes.UniqueViolation,
            ConstraintName: "ux_products_name_lower"
        };
}
