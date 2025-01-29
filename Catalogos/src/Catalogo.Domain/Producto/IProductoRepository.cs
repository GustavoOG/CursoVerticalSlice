namespace Catalogo.Domain.Products;

public interface IProductoRepository
{
    Task<Producto?> GetByCode(string code, CancellationToken cancellationToken);
    Task<List<Producto>> GetAll(CancellationToken cancellationToken);
    void Add(Producto product);
    Task<Producto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}