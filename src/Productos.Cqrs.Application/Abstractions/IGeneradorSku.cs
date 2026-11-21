using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Abstractions;

/// <summary>
/// Puerto hacia un servicio externo (ej. el catalogo corporativo que asigna el SKU).
/// Application define el contrato; Infrastructure provee la implementacion real o simulada.
/// </summary>
public interface IGeneradorSku
{
    Task<string> GenerarSkuAsync(CategoriaProducto categoria, CancellationToken ct = default);
}
