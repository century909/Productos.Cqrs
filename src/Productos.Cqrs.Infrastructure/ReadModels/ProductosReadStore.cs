using Microsoft.EntityFrameworkCore;
using Productos.Cqrs.Application.Abstractions.Data;
using Productos.Cqrs.Application.Productos;
using Productos.Cqrs.Infrastructure.Persistence;

namespace Productos.Cqrs.Infrastructure.ReadModels;

/// <summary>
/// Lado de LECTURA: proyecciones directas a DTO, sin tracking y sin pasar por el agregado.
/// En produccion puede apuntar a una replica de lectura, una vista materializada o Dapper.
/// </summary>
internal sealed class ProductosReadStore(ProductosDbContext db) : IProductosReadStore
{
    public Task<ProductoResponse?> ObtenerAsync(int id, CancellationToken ct = default)
        => db.Productos.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductoResponse(
                p.Id, p.Nombre, p.Descripcion, p.Precio, p.Stock,
                p.Precio * p.Stock, p.FechaCreacion, p.FechaActualizacion))
            .FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyList<ProductoResponse>> ListarAsync(string? nombre = null, CancellationToken ct = default)
    {
        var query = db.Productos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            var patron = $"%{nombre.Trim()}%";
            query = query.Where(p => EF.Functions.ILike(p.Nombre, patron));
        }

        return await query
            .OrderBy(p => p.Id)
            .Select(p => new ProductoResponse(
                p.Id, p.Nombre, p.Descripcion, p.Precio, p.Stock,
                p.Precio * p.Stock, p.FechaCreacion, p.FechaActualizacion))
            .ToListAsync(ct);
    }
}
