using Productos.Cqrs.Application.Abstractions.Data;
using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Application.Common;

namespace Productos.Cqrs.Application.Productos.Queries;

public sealed record ListarMovimientosQuery(Guid ProductoId) : IQuery<IReadOnlyList<MovimientoStockResponse>>;

internal sealed class ListarMovimientosQueryHandler(IProductosReadStore readStore)
    : IQueryHandler<ListarMovimientosQuery, IReadOnlyList<MovimientoStockResponse>>
{
    public async Task<IReadOnlyList<MovimientoStockResponse>> Handle(ListarMovimientosQuery query, CancellationToken ct)
        => await readStore.ListarMovimientosAsync(query.ProductoId, ct) ?? throw NotFoundException.Producto(query.ProductoId);
}
