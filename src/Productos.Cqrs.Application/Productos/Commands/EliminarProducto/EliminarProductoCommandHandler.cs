using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Application.Common;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Productos.Commands.EliminarProducto;

internal sealed class EliminarProductoCommandHandler(
    IProductoRepository repository,
    IUnitOfWork unitOfWork) : ICommandHandler<EliminarProductoCommand, int>
{
    public async Task<int> Handle(EliminarProductoCommand command, CancellationToken ct)
    {
        var producto = await repository.ObtenerPorIdAsync(command.ProductoId, ct)
            ?? throw NotFoundException.Producto(command.ProductoId);

        repository.Eliminar(producto);

        await unitOfWork.SaveChangesAsync(ct);
        return producto.Id;
    }
}
