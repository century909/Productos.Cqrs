using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Infrastructure.Persistence.Configurations;

/// <summary>
/// El mapeo ORM vive en Infrastructure: el dominio queda 100% libre de atributos de EF.
/// Se ajusta a la unica tabla existente: public.products del script 01_create_tables.sql.
/// </summary>
internal sealed class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("products");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Nombre)
            .HasColumnName("name")
            .HasMaxLength(Producto.NombreMaxLength)
            .IsRequired();

        builder.Property(p => p.Descripcion)
            .HasColumnName("description")
            .HasMaxLength(Producto.DescripcionMaxLength);

        builder.Property(p => p.Precio)
            .HasColumnName("price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.Stock)
            .HasColumnName("stock")
            .IsRequired();

        builder.Property(p => p.FechaCreacion)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(p => p.FechaActualizacion)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone");

        builder.Ignore(p => p.ValorInventario);
    }
}
