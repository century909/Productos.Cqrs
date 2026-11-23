using FluentValidation;

namespace Productos.Cqrs.Application.Productos.Commands.ActualizarPrecio;

/// <summary>Validacion de formato (400). Las reglas de negocio quedan en el agregado (422).</summary>
internal sealed class ActualizarPrecioCommandValidator : AbstractValidator<ActualizarPrecioCommand>
{
    public ActualizarPrecioCommandValidator()
    {
        RuleFor(x => x.ProductoId).NotEmpty();
        RuleFor(x => x.Precio).GreaterThan(0);
    }
}
