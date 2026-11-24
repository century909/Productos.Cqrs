using FluentValidation;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Productos.Commands.ActualizarProducto;

internal sealed class ActualizarProductoCommandValidator : AbstractValidator<ActualizarProductoCommand>
{
    public ActualizarProductoCommandValidator(IProductoRepository repository)
    {
        RuleFor(x => x.ProductoId).GreaterThan(0);

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(Producto.NombreMaxLength)
            .MustAsync(async (command, nombre, ct) =>
                !await repository.ExisteNombreAsync(nombre, command.ProductoId, ct))
            .WithMessage("Ya existe otro producto con ese nombre.");

        RuleFor(x => x.Descripcion).MaximumLength(Producto.DescripcionMaxLength);

        RuleFor(x => x.Precio)
            .GreaterThanOrEqualTo(Producto.PrecioMinimo)
            .LessThanOrEqualTo(Producto.PrecioMaximo)
            .PrecisionScale(18, 2, ignoreTrailingZeros: false);
    }
}
