using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Infrastructure.Persistence;

/// <summary>Datos de ejemplo. Se crean usando el propio dominio, respetando sus reglas.</summary>
public static class DatosIniciales
{
    public static readonly Guid ProductoElectronicaId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid ProductoDescontinuadoId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid ProductoAlimentoId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    internal static void Cargar(ProductosDbContext db)
    {
        if (db.Productos.Any())
            return;

        var hoy = DateTime.UtcNow;

        var auriculares = Producto.Crear(ProductoElectronicaId, "ELE-0001", "Auriculares Bluetooth",
            "Auriculares inalambricos con cancelacion de ruido", CategoriaProducto.Electronica,
            120_000m, 25, hoy.AddMonths(-8));
        auriculares.AjustarStock(-5, "Venta mostrador", hoy.AddDays(-3));

        var cafetera = Producto.Crear(ProductoDescontinuadoId, "HOG-0002", "Cafetera Express",
            null, CategoriaProducto.Hogar, 450_000m, 0, hoy.AddMonths(-14));
        cafetera.Descontinuar("El proveedor dejo de fabricarlo");

        var arroz = Producto.Crear(ProductoAlimentoId, "ALI-0003", "Arroz Integral 1 kg",
            "Bolsa de 1 kg", CategoriaProducto.Alimentos, 3_500m, 200, hoy.AddMonths(-2));
        arroz.AjustarStock(50, "Reposicion de proveedor", hoy.AddDays(-10));

        db.Productos.AddRange(auriculares, cafetera, arroz);
        db.SaveChanges();
    }
}
