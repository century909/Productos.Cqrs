namespace Productos.Cqrs.Domain.Productos;

public enum EstadoProducto
{
    Activo = 1,
    Descontinuado = 2
}

public enum CategoriaProducto
{
    Electronica = 1,
    Alimentos = 2,
    Ropa = 3,
    Hogar = 4
}

public enum TipoMovimientoStock
{
    Entrada = 1,
    Salida = 2
}
