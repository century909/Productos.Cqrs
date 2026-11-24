using Productos.Cqrs.Application.Abstractions.Messaging;

namespace Productos.Cqrs.Application.Productos.Commands.AjustarStock;

/// <summary>
/// Cantidad firmada: positiva registra una entrada, negativa una salida.
/// Actualiza la columna stock de products (no existe tabla de movimientos).
/// </summary>
public sealed record AjustarStockCommand(int ProductoId, int Cantidad) : ICommand<ProductoResponse>;
