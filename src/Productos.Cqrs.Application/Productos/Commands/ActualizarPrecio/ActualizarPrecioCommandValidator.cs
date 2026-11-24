using FluentValidation;
using Productos.Cqrs.Domain.Productos;

namespace Productos.Cqrs.Application.Productos.Commands.ActualizarPrecio;

/// <summary>Validacion de formato (400). Las reglas de negocio quedan en el agregado (422).</summary>
internal sealed class ActualizarPrecioCommandValidator : AbstractValidator<ActualizarPrecioCommand>
{
    public ActualizarPrecioCommandValidator()
    {
        RuleFor(x => x.ProductoId).GreaterThan(0);
        RuleFor(x => x.Precio)
            .GreaterThanOrEqualTo(Producto.PrecioMinimo)
            .LessThanOrEqualTo(Producto.PrecioMaximo)
            .PrecisionScale(18, 2, ignoreTrailingZeros: false);
    }
}
