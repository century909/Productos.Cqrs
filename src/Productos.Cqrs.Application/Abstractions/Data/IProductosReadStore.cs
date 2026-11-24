using Productos.Cqrs.Application.Productos;

namespace Productos.Cqrs.Application.Abstractions.Data;

/// <summary>
/// Puerto del MODELO DE LECTURA: devuelve DTOs listos para la respuesta, optimizados para consultar.
/// No pasa por el agregado ni aplica reglas de negocio.
/// </summary>
public interface IProductosReadStore
{
    Task<ProductoResponse?> ObtenerAsync(int id, CancellationToken ct = default);

    /// <summary>Filtra por coincidencia parcial de nombre cuando se indica.</summary>
    Task<IReadOnlyList<ProductoResponse>> ListarAsync(string? nombre = null, CancellationToken ct = default);
}
