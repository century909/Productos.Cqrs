using FluentValidation;

namespace Productos.Cqrs.Application.Productos.Commands.DescontinuarProducto;

/// <summary>Validacion de formato (400). Las reglas de negocio quedan en el agregado (422).</summary>
internal sealed class DescontinuarProductoCommandValidator : AbstractValidator<DescontinuarProductoCommand>
{
    public DescontinuarProductoCommandValidator()
    {
        RuleFor(x => x.ProductoId).NotEmpty();
        RuleFor(x => x.Motivo).NotEmpty().MaximumLength(200);
    }
}
