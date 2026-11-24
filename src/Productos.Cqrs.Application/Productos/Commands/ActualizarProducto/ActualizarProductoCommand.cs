using Productos.Cqrs.Application.Abstractions.Messaging;

namespace Productos.Cqrs.Application.Productos.Commands.ActualizarProducto;

public sealed record ActualizarProductoCommand(
    int ProductoId,
    string Nombre,
    string? Descripcion,
    decimal Precio) : ICommand<ProductoResponse>;
