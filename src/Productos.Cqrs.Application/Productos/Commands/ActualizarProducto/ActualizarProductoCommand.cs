using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Productos.Commands.ActualizarProducto;

public sealed record ActualizarProductoCommand(
    Guid ProductoId,
    string Nombre,
    string? Descripcion,
    CategoriaProducto Categoria,
    decimal Precio) : ICommand<ProductoResponse>;