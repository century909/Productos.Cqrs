using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Application.Productos.Commands.CrearProducto;
using Productos.Cqrs.Application.Productos.Queries;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Api.Endpoints;

/// <summary>
/// La presentacion solo conoce ISender: GET -> Query, POST -> Command.
/// No sabe que handler se ejecuta ni que behaviors lo envuelven.
/// </summary>
public static class ProductosEndpoints
{
    public static IEndpointRouteBuilder MapProductosEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/productos").WithTags("Productos");

        // ---------- Queries ----------
        group.MapGet("/", async (CategoriaProducto? categoria, ISender sender, CancellationToken ct)
                => TypedResults.Ok(await sender.Send(new ListarProductosQuery(categoria), ct)))
            .WithName("ListarProductos");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct)
                => TypedResults.Ok(await sender.Send(new ObtenerProductoQuery(id), ct)))
            .WithName("ObtenerProducto")
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/{id:guid}/movimientos", async (Guid id, ISender sender, CancellationToken ct)
                => TypedResults.Ok(await sender.Send(new ListarMovimientosQuery(id), ct)))
            .WithName("ListarMovimientos")
            .ProducesProblem(StatusCodes.Status404NotFound);

        // ---------- Commands ----------
        group.MapPost("/", async (CrearProductoCommand command, ISender sender, CancellationToken ct) =>
            {
                var producto = await sender.Send(command, ct);
                return TypedResults.CreatedAtRoute(producto, "ObtenerProducto", new { id = producto.Id });
            })
            .WithName("CrearProducto")
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return app;
    }
}
