namespace Catalogo.Domain.Categories;

public interface ICategoriaRepository
{
    Task<List<Categoria>> GetAll(CancellationToken cancellationToken);
    void Add(Categoria category);
    Task<Categoria?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}