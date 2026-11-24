using Productos.Cqrs.Domain.Common;

namespace Productos.Cqrs.Domain.Productos;

/// <summary>
/// Agregado raiz. Modelo rico: el estado solo cambia a traves de metodos que protegen las invariantes.
/// Refleja exactamente la tabla public.products (id, name, description, price, stock, created_at, updated_at).
/// No conoce EF Core, HTTP ni ninguna tecnologia: es C# puro y se prueba sin infraestructura.
/// </summary>
public sealed class Producto : Entity
{
    public const int NombreMaxLength = 150;
    public const int DescripcionMaxLength = 500;
    public const decimal PrecioMinimo = 0m;

    // NUMERIC(18,2): 16 digitos enteros + 2 decimales.
    public const decimal PrecioMaximo = 9_999_999_999_999_999.99m;

    private Producto() { } // Requerido por el ORM

    public string Nombre { get; private set; } = null!;
    public string? Descripcion { get; private set; }
    public decimal Precio { get; private set; }
    public int Stock { get; private set; }
    public DateTimeOffset FechaCreacion { get; private set; }
    public DateTimeOffset? FechaActualizacion { get; private set; }

    public decimal ValorInventario => Precio * Stock;

    public static Producto Crear(
        string nombre, string? descripcion, decimal precio, int stockInicial, DateTimeOffset ahora)
    {
        ValidarNombre(nombre);
        ValidarDescripcion(descripcion);
        ValidarPrecio(precio);
        if (stockInicial < 0)
            throw new DomainException("El stock inicial no puede ser negativo.");

        return new Producto
        {
            Nombre = nombre.Trim(),
            Descripcion = NormalizarDescripcion(descripcion),
            Precio = precio,
            Stock = stockInicial,
            FechaCreacion = ahora
        };
    }

    public void Actualizar(string nombre, string? descripcion, decimal precio, DateTimeOffset ahora)
    {
        ValidarNombre(nombre);
        ValidarDescripcion(descripcion);
        ValidarPrecio(precio);

        Nombre = nombre.Trim();
        Descripcion = NormalizarDescripcion(descripcion);
        Precio = precio;
        FechaActualizacion = ahora;
    }

    public void ActualizarPrecio(decimal precio, DateTimeOffset ahora)
    {
        ValidarPrecio(precio);
        Precio = precio;
        FechaActualizacion = ahora;
    }

    /// <summary>Cantidad firmada: positiva registra una entrada, negativa una salida.</summary>
    public void AjustarStock(int cantidad, DateTimeOffset ahora)
    {
        if (cantidad == 0)
            throw new DomainException("La cantidad del ajuste no puede ser cero.");
        if (Stock + cantidad < 0)
            throw new DomainException("Stock insuficiente para registrar la salida.");

        Stock += cantidad;
        FechaActualizacion = ahora;
    }

    private static void ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DomainException("El nombre es obligatorio.");
        if (nombre.Trim().Length > NombreMaxLength)
            throw new DomainException($"El nombre no puede superar los {NombreMaxLength} caracteres.");
    }

    private static void ValidarDescripcion(string? descripcion)
    {
        if (descripcion?.Trim().Length > DescripcionMaxLength)
            throw new DomainException($"La descripcion no puede superar los {DescripcionMaxLength} caracteres.");
    }

    private static void ValidarPrecio(decimal precio)
    {
        if (precio is < PrecioMinimo or > PrecioMaximo)
            throw new DomainException($"El precio debe estar entre {PrecioMinimo:N2} y {PrecioMaximo:N2}.");
    }

    private static string? NormalizarDescripcion(string? descripcion)
        => string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim();
}
