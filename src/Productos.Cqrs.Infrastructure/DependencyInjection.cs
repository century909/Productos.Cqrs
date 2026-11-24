using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Productos.Cqrs.Application.Abstractions.Data;
using Productos.Cqrs.Domain.Productos;
using Productos.Cqrs.Infrastructure.Persistence;
using Productos.Cqrs.Infrastructure.ReadModels;

namespace Productos.Cqrs.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // PostgreSQL: base bootcamp_db y tabla public.products creadas con 01_create_tables.sql.
        // Con CQRS el lado de lectura podria usar otra conexion (replica) u otro motor.
        services.AddDbContext<ProductosDbContext>(options => options.UseNpgsql(connectionString));

        // Escritura
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ProductosDbContext>());
        services.AddScoped<IProductoRepository, ProductoRepository>();

        // Lectura
        services.AddScoped<IProductosReadStore, ProductosReadStore>();

        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
