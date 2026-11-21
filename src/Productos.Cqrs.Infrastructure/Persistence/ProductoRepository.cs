using Microsoft.EntityFrameworkCore;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Infrastructure.Persistence;

/// <summary>Lado de escritura: entidades con tracking para que EF detecte los cambios del agregado.</summary>
internal sealed class ProductoRepository(ProductosDbContext db) : IProductoRepository
{
    public Task<Producto?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default)
        => db.Productos.FirstOrDefaultAsync(p => p.Id == id, ct);

    public void Agregar(Producto producto) => db.Productos.Add(producto);
}
