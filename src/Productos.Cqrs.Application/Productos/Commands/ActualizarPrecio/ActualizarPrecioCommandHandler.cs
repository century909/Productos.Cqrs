using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Application.Common;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Productos.Commands.ActualizarPrecio;

internal sealed class ActualizarPrecioCommandHandler(
    IProductoRepository repository,
    IUnitOfWork unitOfWork) : ICommandHandler<ActualizarPrecioCommand, ProductoResponse>
{
    public async Task<ProductoResponse> Handle(ActualizarPrecioCommand command, CancellationToken ct)
    {
        var producto = await repository.ObtenerPorIdAsync(command.ProductoId, ct)
            ?? throw NotFoundException.Producto(command.ProductoId);

        producto.ActualizarPrecio(command.Precio);

        await unitOfWork.SaveChangesAsync(ct);
        return producto.ToResponse();
    }
}
