using Productos.Cqrs.Application.Abstractions.Data;
using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Application.Common;

namespace Productos.Cqrs.Application.Productos.Queries;

public sealed record ObtenerProductoQuery(int ProductoId) : IQuery<ProductoResponse>;

/// <summary>Lado de lectura: va directo al read store, no carga el agregado.</summary>
internal sealed class ObtenerProductoQueryHandler(IProductosReadStore readStore)
    : IQueryHandler<ObtenerProductoQuery, ProductoResponse>
{
    public async Task<ProductoResponse> Handle(ObtenerProductoQuery query, CancellationToken ct)
        => await readStore.ObtenerAsync(query.ProductoId, ct) ?? throw NotFoundException.Producto(query.ProductoId);
}
