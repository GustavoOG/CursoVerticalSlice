using CarritoCompras.Api.Compartidos.NetWorking.CatalogoApi;
using CarritoCompras.Api.Compartidos.Slices;

namespace CarritoCompras.Api.Componentes.Catalogo
{
    public sealed class BuscarProducto : ISlice
    {
        public void AgregaEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/catalogo/{codigo}",
                async (string codigo,
                ILoggerFactory loggerFactory,
                    ICatalogoApiCliente catalogoApiCliente,
                    CancellationToken cancellationToken) =>
                {
                    loggerFactory.CreateLogger("EndpointCatalogo-codigo").
                    LogInformation("Buscar Cátalogo por Código");

                    var result = await catalogoApiCliente.ObtieneProductoPorcodigoAsync(codigo, cancellationToken);
                    return Results.Ok(result);
                });
        }
    }
}
