using Productos.Cqrs.Application.Abstractions.Messaging;

namespace Productos.Cqrs.Application.Productos.Commands.CrearProducto;

public sealed record CrearProductoCommand(
    string Nombre,
    string? Descripcion,
    decimal Precio,
    int StockInicial) : ICommand<ProductoResponse>;
