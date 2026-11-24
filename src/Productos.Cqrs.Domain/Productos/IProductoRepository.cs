namespace Productos.Cqrs.Domain.Productos;

/// <summary>
/// Repositorio del LADO DE ESCRITURA: solo carga y guarda el agregado para aplicar reglas.
/// Las lecturas van por un modelo de lectura separado (IProductosReadStore en Application).
/// </summary>
public interface IProductoRepository
{
    Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Comprueba la regla de unicidad del indice ux_products_name_lower (lower(name)).
    /// La API la valida antes de escribir para responder 400 en lugar de un error de base de datos.
    /// </summary>
    Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null, CancellationToken ct = default);

    void Agregar(Producto producto);
    void Eliminar(Producto producto);
}

/// <summary>Confirma los cambios del comando como una unidad.</summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
