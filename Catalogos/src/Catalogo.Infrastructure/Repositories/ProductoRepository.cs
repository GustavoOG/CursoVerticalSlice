using Catalogo.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Infrastructure.Repositories;

internal sealed class ProductoRepository : Repository<Producto>, IProductoRepository
{
    public ProductoRepository(CatalogoDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Producto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.Set<Producto>().ToListAsync(cancellationToken);
    }

    public async Task<Producto?> GetByCode(string code, CancellationToken cancellationToken)
    {
        return await dbContext
                     .Set<Producto>()
                     .Where(x => x.Code == code)
                     .FirstOrDefaultAsync(cancellationToken);
    }
}
