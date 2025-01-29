using Catalogo.Domain.Abstractions;

namespace Catalogo.Domain.Categories.Events;

public sealed record CategoriaCreatedDomainEvent(Guid categoryId) : IDomainEvent;