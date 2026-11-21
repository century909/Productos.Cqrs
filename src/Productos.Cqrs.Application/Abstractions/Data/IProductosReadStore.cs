using Productos.Cqrs.Application.Productos;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Abstractions.Data;

/// <summary>
/// Puerto del MODELO DE LECTURA: devuelve DTOs listos para la respuesta, optimizados para consultar.
/// No pasa por el agregado ni aplica reglas de negocio.
/// </summary>
public interface IProductosReadStore
{
    Task<ProductoResponse?> ObtenerAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<ProductoResponse>> ListarAsync(CategoriaProducto? categoria, CancellationToken ct = default);

    /// <returns>null si el producto no existe.</returns>
    Task<IReadOnlyList<MovimientoStockResponse>?> ListarMovimientosAsync(Guid productoId, CancellationToken ct = default);
}
