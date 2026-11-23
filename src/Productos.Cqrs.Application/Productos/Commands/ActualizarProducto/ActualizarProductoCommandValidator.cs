using FluentValidation;

namespace Productos.Cqrs.Application.Productos.Commands.ActualizarProducto;

internal sealed class ActualizarProductoCommandValidator : AbstractValidator<ActualizarProductoCommand>
{
    public ActualizarProductoCommandValidator()
    {
        RuleFor(x => x.ProductoId).NotEmpty();
        RuleFor(x => x.Nombre).NotEmpty().Length(3, 120);
        RuleFor(x => x.Descripcion).MaximumLength(500);
        RuleFor(x => x.Categoria).IsInEnum();
        RuleFor(x => x.Precio).GreaterThan(0);
    }
}