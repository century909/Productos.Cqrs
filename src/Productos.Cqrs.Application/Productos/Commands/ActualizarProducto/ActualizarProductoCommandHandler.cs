using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Application.Common;
using Productos.Cqrs.Domain.Productos;
 namespace Productos.Cqrs.Application.Productos.Commands.ActualizarProducto;
internal sealed class ActualizarProductoCommandHandler(
    IProductoRepository repository,
    IUnitOfWork unitOfWork) : ICommandHandler<ActualizarProductoCommand, ProductoResponse>
{
    public async Task<ProductoResponse> Handle(ActualizarProductoCommand command, CancellationToken ct)
    {
        var producto = await repository.ObtenerPorIdAsync(command.ProductoId, ct)
            ?? throw NotFoundException.Producto(command.ProductoId);

        producto.Actualizar(command.Nombre, command.Descripcion, command.Categoria, command.Precio);

        await unitOfWork.SaveChangesAsync(ct);
        return producto.ToResponse();
    }
}