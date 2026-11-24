namespace Productos.Cqrs.Application.Common;

/// <summary>El recurso solicitado no existe (HTTP 404).</summary>
public class NotFoundException(string message) : Exception(message)
{
    public static NotFoundException Producto(int id) => new($"No existe el producto {id}.");
}
