using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Application.Common;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Productos.Commands.AjustarStock;

internal sealed class AjustarStockCommandHandler(
    IProductoRepository repository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : ICommandHandler<AjustarStockCommand, ProductoResponse>
{
    public async Task<ProductoResponse> Handle(AjustarStockCommand command, CancellationToken ct)
    {
        var producto = await repository.ObtenerPorIdAsync(command.ProductoId, ct)
            ?? throw NotFoundException.Producto(command.ProductoId);

        producto.AjustarStock(command.Cantidad, timeProvider.GetUtcNow());

        await unitOfWork.SaveChangesAsync(ct);
        return producto.ToResponse();
    }
}
