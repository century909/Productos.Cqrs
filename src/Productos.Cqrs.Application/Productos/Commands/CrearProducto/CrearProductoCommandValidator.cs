using FluentValidation;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Productos.Commands.CrearProducto;

/// <summary>
/// Validacion de formato (400). Se ejecuta automaticamente en el ValidationBehavior.
/// La unicidad de lower(name) se valida contra el repositorio para responder 400
/// antes de que el indice ux_products_name_lower lance un error de base de datos.
/// </summary>
internal sealed class CrearProductoCommandValidator : AbstractValidator<CrearProductoCommand>
{
    public CrearProductoCommandValidator(IProductoRepository repository)
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(Producto.NombreMaxLength)
            .MustAsync(async (nombre, ct) => !await repository.ExisteNombreAsync(nombre, null, ct))
            .WithMessage("Ya existe un producto con ese nombre.");

        RuleFor(x => x.Descripcion).MaximumLength(Producto.DescripcionMaxLength);

        RuleFor(x => x.Precio)
            .GreaterThanOrEqualTo(Producto.PrecioMinimo)
            .LessThanOrEqualTo(Producto.PrecioMaximo)
            .PrecisionScale(18, 2, ignoreTrailingZeros: false);

        RuleFor(x => x.StockInicial).GreaterThanOrEqualTo(0);
    }
}
