using Productos.Cqrs.Application.Abstractions.Messaging;

namespace Productos.Cqrs.Application.Productos.Commands.DescontinuarProducto;

public sealed record DescontinuarProductoCommand(Guid ProductoId, string Motivo) : ICommand<ProductoResponse>;
