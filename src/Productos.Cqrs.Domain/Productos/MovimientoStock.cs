using Productos.Cqrs.Domain.Common;

namespace Productos.Cqrs.Domain.Productos;

/// <summary>Entidad hija del agregado Producto: solo se crea a traves del producto.</summary>
public sealed class MovimientoStock : Entity
{
    private MovimientoStock() { } // Requerido por el ORM

    public Guid ProductoId { get; private set; }
    public TipoMovimientoStock Tipo { get; private set; }

    /// <summary>Cantidad firmada: positiva en entradas, negativa en salidas.</summary>
    public int Cantidad { get; private set; }

    public string Motivo { get; private set; } = null!;
    public DateTime Fecha { get; private set; }

    internal static MovimientoStock Crear(Guid productoId, TipoMovimientoStock tipo, int cantidad, string motivo, DateTime fecha)
        => new()
        {
            Id = Guid.CreateVersion7(),
            ProductoId = productoId,
            Tipo = tipo,
            Cantidad = cantidad,
            Motivo = motivo,
            Fecha = fecha
        };
}
