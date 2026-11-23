using Productos.Cqrs.Application.Abstractions.Messaging;

namespace Productos.Cqrs.Application.Productos.Commands.ActualizarPrecio;

public sealed record ActualizarPrecioCommand(Guid ProductoId, decimal Precio) : ICommand<ProductoResponse>;
