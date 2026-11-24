namespace Productos.Cqrs.Domain.Common;

public abstract class Entity
{
    /// <summary>Clave generada por la base de datos (products.id es IDENTITY).</summary>
    public int Id { get; protected set; }
}

/// <summary>Violacion de una regla de negocio del dominio.</summary>
public class DomainException(string message) : Exception(message);
