using FluentValidation;

namespace Productos.Cqrs.Application.Productos.Commands.EliminarProducto;

internal sealed class EliminarProductoCommandValidator : AbstractValidator<EliminarProductoCommand>
{
    public EliminarProductoCommandValidator()
    {
        RuleFor(x => x.ProductoId).GreaterThan(0);
    }
}
