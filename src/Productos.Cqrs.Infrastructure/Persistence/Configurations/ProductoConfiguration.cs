using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Infrastructure.Persistence.Configurations;

/// <summary>
/// El mapeo ORM vive en Infrastructure: el dominio queda 100% libre de atributos de EF
/// (evita el anti-patron "fuga de infraestructura").
/// </summary>
internal sealed class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.Sku).HasMaxLength(20).IsRequired();
        builder.Property(p => p.Nombre).HasMaxLength(120).IsRequired();
        builder.Property(p => p.Descripcion).HasMaxLength(500);
        builder.Property(p => p.Precio).HasPrecision(18, 2);
        builder.Property(p => p.MotivoDescontinuacion).HasMaxLength(200);
        builder.Ignore(p => p.ValorInventario);

        builder.HasMany(p => p.Movimientos).WithOne().HasForeignKey(m => m.ProductoId);
        builder.Navigation(p => p.Movimientos).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

internal sealed class MovimientoStockConfiguration : IEntityTypeConfiguration<MovimientoStock>
{
    public void Configure(EntityTypeBuilder<MovimientoStock> builder)
    {
        builder.HasKey(m => m.Id);
        // El Id lo genera el dominio: ValueGeneratedNever hace que EF trate los movimientos nuevos como INSERT.
        builder.Property(m => m.Id).ValueGeneratedNever();
        builder.Property(m => m.Motivo).HasMaxLength(200).IsRequired();
    }
}
