using CarritoCompras.Api.Compartidos.Domain.Models;

namespace CarritoCompras.Api.Compartidos.NetWorking.CatalogoApi
{
    public interface ICatalogoApiCliente
    {
        Task<IEnumerable<Catalogo?>> ObtieneProductosAsync(CancellationToken cancellationToken);

        Task<Catalogo> ObtieneProductoPorcodigoAsync(string Codigo, CancellationToken cancellationToken);

    }
}
