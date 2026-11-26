namespace Productos.Cqrs.Application.Common;

public sealed class NombreProductoDuplicadoException(Exception inner)
    : Exception("Ya existe un producto con el mismo nombre.", inner);
