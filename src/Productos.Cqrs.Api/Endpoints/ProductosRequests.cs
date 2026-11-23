namespace Productos.Cqrs.Api.Endpoints;

// Cuerpos HTTP separados de los commands: Application no conoce contratos de la API.
// El Id del producto viaja en la ruta, no en el body.

public sealed record DescontinuarProductoRequest(string Motivo);

/// <summary>Cantidad firmada: positiva entrada, negativa salida.</summary>
public sealed record AjustarStockRequest(int Cantidad, string Motivo);
