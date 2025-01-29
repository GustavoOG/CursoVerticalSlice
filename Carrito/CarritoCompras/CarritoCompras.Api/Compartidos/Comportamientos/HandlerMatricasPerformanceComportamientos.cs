using CarritoCompras.Api.Compartidos.Metricas;
using MediatR;
using System.Diagnostics;

namespace CarritoCompras.Api.Compartidos.Comportamientos
{
    public sealed class HandlerMatricasPerformanceComportamientos<TRequest, TReponse>(HandlerMatricasPerformance handlerMatricasPerformance)
        : IPipelineBehavior<TRequest, TReponse> where TRequest : IRequest<TReponse>
    {
        private readonly Stopwatch _timer = new Stopwatch();
        private readonly HandlerMatricasPerformance _handlerMatricasPerformance = handlerMatricasPerformance;

        public async Task<TReponse> Handle(TRequest request,
            RequestHandlerDelegate<TReponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();
            var response = await next();
            _timer.Stop();
            _handlerMatricasPerformance.MilliSecondsElapsed(_timer.ElapsedMilliseconds);
            return response;
        }
    }
}
