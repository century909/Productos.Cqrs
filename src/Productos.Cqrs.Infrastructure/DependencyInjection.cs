using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Productos.Cqrs.Application.Abstractions;
using Productos.Cqrs.Application.Abstractions.Data;
using Productos.Cqrs.Domain.Productos;
using Productos.Cqrs.Infrastructure.ExternalServices;
using Productos.Cqrs.Infrastructure.Persistence;
using Productos.Cqrs.Infrastructure.ReadModels;

namespace Productos.Cqrs.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string databaseName = "ProductosDb")
    {
        // Para PostgreSQL: options.UseNpgsql(connectionString) + paquete Npgsql.EntityFrameworkCore.PostgreSQL.
        // Con CQRS el lado de lectura podria usar otra conexion (replica) u otro motor.
        services.AddDbContext<ProductosDbContext>(options => options.UseInMemoryDatabase(databaseName));

        // Escritura
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ProductosDbContext>());
        services.AddScoped<IProductoRepository, ProductoRepository>();

        // Lectura
        services.AddScoped<IProductosReadStore, ProductosReadStore>();

        services.AddSingleton<IGeneradorSku, GeneradorSkuSimulado>();
        services.AddSingleton(TimeProvider.System);

        return services;
    }

    public static void InicializarBaseDeDatos(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProductosDbContext>();
        db.Database.EnsureCreated();
        DatosIniciales.Cargar(db);
    }
}
