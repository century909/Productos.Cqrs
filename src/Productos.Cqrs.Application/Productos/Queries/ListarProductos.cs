using Productos.Cqrs.Application.Abstractions.Data;
using Productos.Cqrs.Application.Abstractions.Messaging;

namespace Productos.Cqrs.Application.Productos.Queries;

public sealed record ListarProductosQuery(string? Nombre) : IQuery<IReadOnlyList<ProductoResponse>>;

internal sealed class ListarProductosQueryHandler(IProductosReadStore readStore)
    : IQueryHandler<ListarProductosQuery, IReadOnlyList<ProductoResponse>>
{
    public Task<IReadOnlyList<ProductoResponse>> Handle(ListarProductosQuery query, CancellationToken ct)
        => readStore.ListarAsync(query.Nombre, ct);
}
