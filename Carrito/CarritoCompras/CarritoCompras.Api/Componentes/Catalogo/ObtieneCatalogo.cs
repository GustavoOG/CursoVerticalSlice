using CarritoCompras.Api.Compartidos.NetWorking.CatalogoApi;
using CarritoCompras.Api.Compartidos.Slices;

namespace CarritoCompras.Api.Componentes.Catalogo
{
    public sealed class ObtieneCatalogo : ISlice
    {
        public void AgregaEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/catalogo",
                async (ILoggerFactory loggerFactory,
                    ICatalogoApiCliente catalogoApiCliente,
                    CancellationToken cancellationToken) =>
                {
                    loggerFactory.CreateLogger("EndpointCatalogo-get").
                    LogInformation("Catalogo de productos");

                    var result = await catalogoApiCliente.ObtieneProductosAsync(cancellationToken);
                    return Results.Ok(result);
                });
        }
    }
}
