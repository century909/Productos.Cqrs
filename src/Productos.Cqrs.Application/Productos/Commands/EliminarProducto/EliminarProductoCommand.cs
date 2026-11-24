using Productos.Cqrs.Application.Abstractions.Messaging;

namespace Productos.Cqrs.Application.Productos.Commands.EliminarProducto;

public sealed record EliminarProductoCommand(int ProductoId) : ICommand<int>;
