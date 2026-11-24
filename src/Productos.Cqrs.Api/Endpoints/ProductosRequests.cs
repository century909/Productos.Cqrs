namespace Productos.Cqrs.Api.Endpoints;

// Cuerpos HTTP separados de los commands: Application no conoce contratos de la API.
// El Id del producto viaja en la ruta, no en el body.

public sealed record ActualizarProductoRequest(
    string Nombre,
    string? Descripcion,
    decimal Precio);

/// <summary>PATCH parcial: solo viaja el campo que se quiere modificar.</summary>
public sealed record ActualizarPrecioRequest(decimal Precio);

/// <summary>Cantidad firmada: positiva entrada, negativa salida.</summary>
public sealed record AjustarStockRequest(int Cantidad);
