using Productos.Cqrs.Application.Abstractions.Messaging;
using Productos.Cqrs.Application.Productos;
using Productos.Cqrs.Application.Productos.Commands.AjustarStock;
using Productos.Cqrs.Application.Productos.Commands.CrearProducto;
using Productos.Cqrs.Application.Productos.Commands.DescontinuarProducto;
using Productos.Cqrs.Application.Productos.Commands.EliminarProducto;
using Productos.Cqrs.Application.Productos.Commands.ActualizarProducto;
using Productos.Cqrs.Application.Productos.Queries;

using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Api.Endpoints;

/// <summary>
/// La presentacion solo conoce ISender: GET -> Query, POST/DELETE -> Command.
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

        group.MapPost("/{id:guid}/movimientos", async (Guid id, AjustarStockRequest request, ISender sender, CancellationToken ct) =>
            {
                var producto = await sender.Send(new AjustarStockCommand(id, request.Cantidad, request.Motivo), ct);
                return TypedResults.Ok(producto);
            })
            .WithName("AjustarStock")
            .Produces<ProductoResponse>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        group.MapPost("/{id:guid}/descontinuar", async (Guid id, DescontinuarProductoRequest request, ISender sender, CancellationToken ct) =>
            {
                var producto = await sender.Send(new DescontinuarProductoCommand(id, request.Motivo), ct);
                return TypedResults.Ok(producto);
            })
            .WithName("DescontinuarProducto")
            .Produces<ProductoResponse>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
        group.MapPut("/{id:guid}", async (Guid id, ActualizarProductoRequest request, ISender sender, CancellationToken ct) =>
            {
                var producto = await sender.Send(
                    new ActualizarProductoCommand(id, request.Nombre, request.Descripcion, request.Categoria, request.Precio), ct);
                return TypedResults.Ok(producto);
            })
            .WithName("ActualizarProducto")
            .Produces<ProductoResponse>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken ct) =>
            {
                await sender.Send(new EliminarProductoCommand(id), ct);
                return TypedResults.NoContent();
            })
            .WithName("EliminarProducto")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        return app;

    }
}
