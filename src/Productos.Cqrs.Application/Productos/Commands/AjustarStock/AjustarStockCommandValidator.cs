using FluentValidation;

namespace Productos.Cqrs.Application.Productos.Commands.AjustarStock;

/// <summary>Validacion de formato (400). El stock insuficiente lo decide el agregado (422).</summary>
internal sealed class AjustarStockCommandValidator : AbstractValidator<AjustarStockCommand>
{
    public AjustarStockCommandValidator()
    {
        RuleFor(x => x.ProductoId).GreaterThan(0);
        RuleFor(x => x.Cantidad).NotEqual(0).WithMessage("La cantidad del ajuste no puede ser cero.");
    }
}
