using Catalogo.Domain.Categories;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Infrastructure.Repositories;

internal sealed class CategoryRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoryRepository(CatalogoDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Categoria>> GetAll(CancellationToken cancellationToken)
    {
       return await (
                    from c in dbContext.Set<Categoria>()
                    select c
                    ).ToListAsync(cancellationToken);
    }
}