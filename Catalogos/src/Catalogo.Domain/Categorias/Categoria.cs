using Catalogo.Domain.Abstractions;
using Catalogo.Domain.Categories.Events;


namespace Catalogo.Domain.Categories;

public class Categoria : Entity
{

    public string? Name { get; private set; }
    private Categoria() { }
    private Categoria(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static Categoria Create(string name)
    {
        var category = new Categoria(Guid.NewGuid(), name);
        var categoryDomainEvent = new CategoriaCreatedDomainEvent(category.Id);
        category.RaiseDomainEvent(categoryDomainEvent);
        return category;
    }
}