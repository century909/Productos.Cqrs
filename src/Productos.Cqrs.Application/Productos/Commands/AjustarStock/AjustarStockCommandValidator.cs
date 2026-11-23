using FluentValidation;

namespace Productos.Cqrs.Application.Productos.Commands.AjustarStock;

/// <summary>Validacion de formato (400). El stock insuficiente lo decide el agregado (422).</summary>
internal sealed class AjustarStockCommandValidator : AbstractValidator<AjustarStockCommand>
{
    public AjustarStockCommandValidator()
    {
        RuleFor(x => x.ProductoId).NotEmpty();
        RuleFor(x => x.Cantidad).NotEqual(0).WithMessage("La cantidad del movimiento no puede ser cero.");
        RuleFor(x => x.Motivo).NotEmpty().MaximumLength(200);
    }
}
