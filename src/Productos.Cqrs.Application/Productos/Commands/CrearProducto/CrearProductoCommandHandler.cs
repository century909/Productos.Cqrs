using Productos.Cqrs.Application.Abstractions;
using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Productos.Commands.CrearProducto;

internal sealed class CrearProductoCommandHandler(
    IGeneradorSku generadorSku,
    IProductoRepository repository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : ICommandHandler<CrearProductoCommand, ProductoResponse>
{
    public async Task<ProductoResponse> Handle(CrearProductoCommand command, CancellationToken ct)
    {
        var sku = await generadorSku.GenerarSkuAsync(command.Categoria, ct);
        var producto = Producto.Crear(
            Guid.CreateVersion7(), sku, command.Nombre, command.Descripcion, command.Categoria,
            command.Precio, command.StockInicial, timeProvider.GetUtcNow().UtcDateTime);

        repository.Agregar(producto);
        await unitOfWork.SaveChangesAsync(ct);

        return producto.ToResponse();
    }
}
