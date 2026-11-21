using FluentValidation;

namespace Productos.Cqrs.Application.Productos.Commands.CrearProducto;

/// <summary>Validacion de formato (400). Se ejecuta automaticamente en el ValidationBehavior.</summary>
internal sealed class CrearProductoCommandValidator : AbstractValidator<CrearProductoCommand>
{
    public CrearProductoCommandValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().Length(3, 120);
        RuleFor(x => x.Descripcion).MaximumLength(500);
        RuleFor(x => x.Categoria).IsInEnum();
        RuleFor(x => x.Precio).GreaterThan(0);
        RuleFor(x => x.StockInicial).GreaterThanOrEqualTo(0);
    }
}
