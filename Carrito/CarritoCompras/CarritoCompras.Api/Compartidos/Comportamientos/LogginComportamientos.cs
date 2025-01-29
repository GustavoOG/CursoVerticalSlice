using MediatR.Pipeline;

namespace CarritoCompras.Api.Compartidos.Comportamientos
{
    public sealed class LogginComportamientos<TRequest>(ILogger<TRequest> logger) :
        IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ILogger<TRequest> _logger = logger;
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando ejecucion de la funcionalidad {feattureRequesteName}"
                , typeof(TRequest).Name
                );
            return Task.CompletedTask;
        }
    }
}
