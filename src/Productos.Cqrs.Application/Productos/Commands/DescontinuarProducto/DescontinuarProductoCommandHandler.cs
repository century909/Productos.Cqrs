using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Application.Common;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Productos.Commands.DescontinuarProducto;

internal sealed class DescontinuarProductoCommandHandler(
    IProductoRepository repository,
    IUnitOfWork unitOfWork) : ICommandHandler<DescontinuarProductoCommand, ProductoResponse>
{
    public async Task<ProductoResponse> Handle(DescontinuarProductoCommand command, CancellationToken ct)
    {
        var producto = await repository.ObtenerPorIdAsync(command.ProductoId, ct)
            ?? throw NotFoundException.Producto(command.ProductoId);

        producto.Descontinuar(command.Motivo);

        await unitOfWork.SaveChangesAsync(ct);
        return producto.ToResponse();
    }
}
