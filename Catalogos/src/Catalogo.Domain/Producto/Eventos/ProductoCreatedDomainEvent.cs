using Catalogo.Domain.Abstractions;

namespace Catalogo.Domain.Products.Events;

public sealed record ProductoCreatedDomainEvent(Guid id) : IDomainEvent;