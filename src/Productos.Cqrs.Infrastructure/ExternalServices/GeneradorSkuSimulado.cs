using Productos.Cqrs.Application.Abstractions;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Infrastructure.ExternalServices;

/// <summary>
/// Simula el catalogo corporativo que asigna codigos de producto. En produccion se reemplaza
/// por un cliente HTTP (ej. HttpClient tipado) sin tocar Domain ni Application.
/// El SKU se compone de un prefijo por categoria y cuatro digitos aleatorios.
/// </summary>
internal sealed class GeneradorSkuSimulado : IGeneradorSku
{
    public Task<string> GenerarSkuAsync(CategoriaProducto categoria, CancellationToken ct = default)
    {
        var prefijo = categoria switch
        {
            CategoriaProducto.Electronica => "ELE",
            CategoriaProducto.Alimentos => "ALI",
            CategoriaProducto.Ropa => "ROP",
            CategoriaProducto.Hogar => "HOG",
            _ => "GEN"
        };

        var digitos = Random.Shared.Next(0, 10_000).ToString("D4");
        return Task.FromResult($"{prefijo}-{digitos}");
    }
}
