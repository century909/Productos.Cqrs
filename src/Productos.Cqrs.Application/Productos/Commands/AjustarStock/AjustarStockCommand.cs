using Productos.Cqrs.Application.Abstractions.Messaging;

namespace Productos.Cqrs.Application.Productos.Commands.AjustarStock;

/// <summary>Cantidad firmada: positiva registra una entrada, negativa una salida.</summary>
public sealed record AjustarStockCommand(Guid ProductoId, int Cantidad, string Motivo) : ICommand<ProductoResponse>;
