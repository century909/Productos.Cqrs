using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Productos;

public record ProductoResponse(
    Guid Id,
    string Sku,
    string Nombre,
    string? Descripcion,
    CategoriaProducto Categoria,
    decimal Precio,
    int Stock,
    decimal ValorInventario,
    EstadoProducto Estado,
    string? MotivoDescontinuacion,
    DateTime FechaCreacion);

public record MovimientoStockResponse(Guid Id, TipoMovimientoStock Tipo, int Cantidad, string Motivo, DateTime Fecha);

internal static class ProductoMappings
{
    public static ProductoResponse ToResponse(this Producto p) => new(
        p.Id, p.Sku, p.Nombre, p.Descripcion, p.Categoria, p.Precio, p.Stock,
        p.ValorInventario, p.Estado, p.MotivoDescontinuacion, p.FechaCreacion);

    public static MovimientoStockResponse ToResponse(this MovimientoStock m) => new(m.Id, m.Tipo, m.Cantidad, m.Motivo, m.Fecha);
}
