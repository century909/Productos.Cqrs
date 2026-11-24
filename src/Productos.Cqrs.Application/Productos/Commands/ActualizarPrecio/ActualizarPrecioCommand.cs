using Productos.Cqrs.Application.Abstractions.Messaging;

namespace Productos.Cqrs.Application.Productos.Commands.ActualizarPrecio;

public sealed record ActualizarPrecioCommand(int ProductoId, decimal Precio) : ICommand<ProductoResponse>;
