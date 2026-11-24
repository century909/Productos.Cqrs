using Microsoft.EntityFrameworkCore;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Infrastructure.Persistence;

/// <summary>Lado de escritura: entidades con tracking para que EF detecte los cambios del agregado.</summary>
internal sealed class ProductoRepository(ProductosDbContext db) : IProductoRepository
{
    public Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        => db.Productos.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return Task.FromResult(false);

        // Mismo criterio que el indice ux_products_name_lower: lower(name).
        var normalizado = nombre.Trim().ToLower();
        return db.Productos.AnyAsync(
            p => p.Nombre.ToLower() == normalizado && (excluirId == null || p.Id != excluirId),
            ct);
    }

    public void Agregar(Producto producto) => db.Productos.Add(producto);

    public void Eliminar(Producto producto) => db.Productos.Remove(producto);
}
