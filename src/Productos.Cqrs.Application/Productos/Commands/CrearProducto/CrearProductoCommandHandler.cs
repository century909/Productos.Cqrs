using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Productos.Commands.CrearProducto;

internal sealed class CrearProductoCommandHandler(
    IProductoRepository repository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : ICommandHandler<CrearProductoCommand, ProductoResponse>
{
    public async Task<ProductoResponse> Handle(CrearProductoCommand command, CancellationToken ct)
    {
        var producto = Producto.Crear(
            command.Nombre, command.Descripcion, command.Precio, command.StockInicial,
            timeProvider.GetUtcNow());

        repository.Agregar(producto);
        await unitOfWork.SaveChangesAsync(ct);

        return producto.ToResponse();
    }
}
