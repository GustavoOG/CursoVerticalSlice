using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CarritoCompras.Api.Compartidos.Excepciones
{
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "La excepcion es: {Message}", exception.Message);
            var detalleprobremas = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = exception.Message,
                Detail = exception.StackTrace
            };

            await httpContext.Response.WriteAsJsonAsync(detalleprobremas, cancellationToken);
            return true;
        }
    }
}
