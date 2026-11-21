using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Productos.Commands.CrearProducto;

public sealed record CrearProductoCommand(
    string Nombre,
    string? Descripcion,
    CategoriaProducto Categoria,
    decimal Precio,
    int StockInicial) : ICommand<ProductoResponse>;
