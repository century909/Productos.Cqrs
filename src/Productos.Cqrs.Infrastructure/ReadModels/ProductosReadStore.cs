using Microsoft.EntityFrameworkCore;
using Productos.Cqrs.Application.Abstractions.Data;
using Productos.Cqrs.Application.Productos;
using Productos.Cqrs.Domain.Productos;
using Productos.Cqrs.Infrastructure.Persistence;

namespace Productos.Cqrs.Infrastructure.ReadModels;

/// <summary>
/// Lado de LECTURA: proyecciones directas a DTO, sin tracking y sin pasar por el agregado.
/// En produccion puede apuntar a una replica de lectura, una vista materializada o Dapper.
/// </summary>
internal sealed class ProductosReadStore(ProductosDbContext db) : IProductosReadStore
{
    public Task<ProductoResponse?> ObtenerAsync(Guid id, CancellationToken ct = default)
        => db.Productos.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductoResponse(
                p.Id, p.Sku, p.Nombre, p.Descripcion, p.Categoria, p.Precio, p.Stock,
                p.Precio * p.Stock, p.Estado, p.MotivoDescontinuacion, p.FechaCreacion))
            .FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyList<ProductoResponse>> ListarAsync(CategoriaProducto? categoria, CancellationToken ct = default)
    {
        var query = db.Productos.AsNoTracking();
        if (categoria is not null)
            query = query.Where(p => p.Categoria == categoria);

        return await query
            .OrderBy(p => p.FechaCreacion)
            .Select(p => new ProductoResponse(
                p.Id, p.Sku, p.Nombre, p.Descripcion, p.Categoria, p.Precio, p.Stock,
                p.Precio * p.Stock, p.Estado, p.MotivoDescontinuacion, p.FechaCreacion))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<MovimientoStockResponse>?> ListarMovimientosAsync(Guid productoId, CancellationToken ct = default)
    {
        if (!await db.Productos.AnyAsync(p => p.Id == productoId, ct))
            return null;

        return await db.Productos.AsNoTracking()
            .Where(p => p.Id == productoId)
            .SelectMany(p => p.Movimientos)
            .OrderByDescending(m => m.Fecha)
            .Select(m => new MovimientoStockResponse(m.Id, m.Tipo, m.Cantidad, m.Motivo, m.Fecha))
            .ToListAsync(ct);
    }
}
