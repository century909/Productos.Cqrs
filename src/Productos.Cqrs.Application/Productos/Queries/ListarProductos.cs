using Productos.Cqrs.Application.Abstractions.Data;
using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Productos.Queries;

public sealed record ListarProductosQuery(CategoriaProducto? Categoria) : IQuery<IReadOnlyList<ProductoResponse>>;

internal sealed class ListarProductosQueryHandler(IProductosReadStore readStore)
    : IQueryHandler<ListarProductosQuery, IReadOnlyList<ProductoResponse>>
{
    public Task<IReadOnlyList<ProductoResponse>> Handle(ListarProductosQuery query, CancellationToken ct)
        => readStore.ListarAsync(query.Categoria, ct);
}
