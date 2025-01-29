using CarritoCompras.Api.Compartidos.Domain.Models;
using Polly;
using Polly.Registry;

namespace CarritoCompras.Api.Compartidos.NetWorking.CatalogoApi
{
    public sealed class CatalogoApiCliente(
        ILoggerFactory loggerFactory,
        CatalogoApiServicio Catalogoservicio,
        ResiliencePipelineProvider<string> pipelineProvider
        ) : ICatalogoApiCliente
    {

        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly CatalogoApiServicio _Catalogoservicio = Catalogoservicio;
        private readonly ResiliencePipelineProvider<string> _pipelineProvider = pipelineProvider;

        public async Task<Catalogo?> ObtieneProductoPorcodigoAsync(string Codigo, CancellationToken cancellationToken)
        {
            var _logger = _loggerFactory.CreateLogger("RetryLog");
            var policy = Policy.Handle<ApplicationException>()
                .WaitAndRetryAsync(3, retryAttemp =>
                {
                    _logger.LogInformation($"Intento: {retryAttemp}");
                    var tiempoReintento = TimeSpan.FromSeconds(Math.Pow(2, retryAttemp));
                    return tiempoReintento;
                });
            var product = await policy.ExecuteAsync(() =>
            _Catalogoservicio.ObtieneCatalogoPorCodigoAsync(Codigo, cancellationToken));
            return product;
        }

        public async Task<IEnumerable<Catalogo>> ObtieneProductosAsync(CancellationToken cancellationToken)
        {
            //var _logger = _loggerFactory.CreateLogger("RetryLog");
            //var policy = Policy.Handle<ApplicationException>()
            //    .WaitAndRetryAsync(3, retryAttemp =>
            //    {
            //        _logger.LogInformation($"Intento: {retryAttemp}");
            //        var tiempoReintento = TimeSpan.FromSeconds(Math.Pow(2, retryAttemp));
            //        return tiempoReintento;
            //    });
            //var products = await policy.ExecuteAsync(() =>
            //_Catalogoservicio.ObtieneProductosAsync(cancellationToken));

            var pipeline = _pipelineProvider.GetPipeline<IEnumerable<Catalogo>>("catalogo-productos");
            var products = await pipeline.ExecuteAsync(
                async token =>
                await _Catalogoservicio.ObtieneProductosAsync(token), cancellationToken);
            return products;
        }
    }
}
