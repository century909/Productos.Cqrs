namespace Productos.Cqrs.Application.Productos;

/// <summary>DTO del modelo de lectura: solo los campos de la tabla products.</summary>
public record ProductoResponse(
    int Id,
    string Nombre,
    string? Descripcion,
    decimal Precio,
    int Stock,
    decimal ValorInventario,
    DateTimeOffset FechaCreacion,
    DateTimeOffset? FechaActualizacion);

internal static class ProductoMappings
{
    public static ProductoResponse ToResponse(this Domain.Productos.Producto p) => new(
        p.Id, p.Nombre, p.Descripcion, p.Precio, p.Stock,
        p.ValorInventario, p.FechaCreacion, p.FechaActualizacion);
}
