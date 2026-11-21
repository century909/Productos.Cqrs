namespace Productos.Cqrs.Domain.Productos;

/// <summary>
/// Repositorio del LADO DE ESCRITURA: solo carga y guarda el agregado para aplicar reglas.
/// Las lecturas van por un modelo de lectura separado (IProductosReadStore en Application).
/// </summary>
public interface IProductoRepository
{
    Task<Producto?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    void Agregar(Producto producto);
}

/// <summary>Confirma los cambios del comando como una unidad.</summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
