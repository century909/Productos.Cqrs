using Productos.Cqrs.Domain.Common;

namespace Productos.Cqrs.Domain.Productos;

/// <summary>
/// Agregado raiz. Modelo rico: el estado solo cambia a traves de metodos que protegen las invariantes.
/// No conoce EF Core, HTTP ni ninguna tecnologia: es C# puro y se prueba sin infraestructura.
/// </summary>
public sealed class Producto : Entity
{
    public const decimal PrecioMinimo = 100m;
    public const decimal PrecioMaximo = 100_000_000m;

    private readonly List<MovimientoStock> _movimientos = [];

    private Producto() { } // Requerido por el ORM

    public string Sku { get; private set; } = null!;
    public string Nombre { get; private set; } = null!;
    public string? Descripcion { get; private set; }
    public CategoriaProducto Categoria { get; private set; }
    public decimal Precio { get; private set; }
    public int Stock { get; private set; }
    public EstadoProducto Estado { get; private set; }
    public string? MotivoDescontinuacion { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    public decimal ValorInventario => Precio * Stock;
    public IReadOnlyCollection<MovimientoStock> Movimientos => _movimientos.AsReadOnly();

    public static Producto Crear(
        Guid id, string sku, string nombre, string? descripcion,
        CategoriaProducto categoria, decimal precio, int stockInicial, DateTime ahora)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException("El SKU es obligatorio.");
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre es obligatorio.");
        if (precio is < PrecioMinimo or > PrecioMaximo)
            throw new DomainException($"El precio debe estar entre {PrecioMinimo:N0} y {PrecioMaximo:N0}.");
        if (stockInicial < 0)
            throw new DomainException("El stock inicial no puede ser negativo.");

        return new Producto
        {
            Id = id,
            Sku = sku.Trim().ToUpperInvariant(),
            Nombre = nombre.Trim(),
            Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim(),
            Categoria = categoria,
            Precio = precio,
            Stock = stockInicial,
            Estado = EstadoProducto.Activo,
            FechaCreacion = ahora
        };
    }

    public void Eliminar()
    {
        if (Estado != EstadoProducto.Descontinuado)
            throw new DomainException($"Solo se pueden eliminar productos descontinuados. Estado actual: {Estado}.");
        if (Stock > 0)
            throw new DomainException("No se puede eliminar un producto con stock positivo.");
        // Se podria agregar un flag IsDeleted para soft delete, pero en este template se elimina fisicamente.
    }

    // AjustarStock y Descontinuar se exponen como commands en Application y como endpoints en la API.
    // El seed (DatosIniciales) tambien los usa para construir estados de ejemplo.

    public MovimientoStock AjustarStock(int cantidad, string motivo, DateTime ahora)
    {
        if (Estado != EstadoProducto.Activo)
            throw new DomainException($"El producto esta {Estado} y no admite movimientos de stock.");
        if (cantidad == 0)
            throw new DomainException("La cantidad del movimiento no puede ser cero.");
        if (Stock + cantidad < 0)
            throw new DomainException("Stock insuficiente para registrar la salida.");
        if (string.IsNullOrWhiteSpace(motivo))
            throw new DomainException("El motivo del movimiento es obligatorio.");

        Stock += cantidad;
        var tipo = cantidad > 0 ? TipoMovimientoStock.Entrada : TipoMovimientoStock.Salida;
        var movimiento = MovimientoStock.Crear(Id, tipo, cantidad, motivo.Trim(), ahora);
        _movimientos.Add(movimiento);
        return movimiento;
    }

    public void Descontinuar(string motivo)
    {
        if (Estado != EstadoProducto.Activo)
            throw new DomainException($"Solo se pueden descontinuar productos activos. Estado actual: {Estado}.");
        if (Stock > 0)
            throw new DomainException("Solo se puede descontinuar un producto sin stock. Ajuste el stock a cero primero.");
        if (string.IsNullOrWhiteSpace(motivo))
            throw new DomainException("El motivo de descontinuacion es obligatorio.");

        Estado = EstadoProducto.Descontinuado;
        MotivoDescontinuacion = motivo.Trim();
    }
    public void Actualizar(string nombre, string? descripcion, CategoriaProducto categoria, decimal precio)
    {
        if(Estado != EstadoProducto.Activo)
            throw new DomainException($"Solo se pueden modificar productos activos. Estado actual: {Estado}.");
        if(string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre del producto es obligatorio.");
        if(precio is < PrecioMinimo or > PrecioMaximo)
            throw new DomainException($"El precio debe estar entre {PrecioMinimo:N0} y {PrecioMaximo:N0}.");
        Nombre = nombre.Trim();
        Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim();
        Categoria = categoria;
        Precio = precio;
    }

}
