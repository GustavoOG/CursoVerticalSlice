using Catalogo.Domain.Products.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalogo.Application.Productos.BuscarProductos
{
    internal sealed class CrearProductosDomainEventHandler : INotificationHandler<ProductoCreatedDomainEvent>
    {
        private readonly ILogger _logger;

        public CrearProductosDomainEventHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CrearProductosDomainEventHandler>();
        }

        public Task Handle(ProductoCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Se ha creado un objeto producto {notification.id}");
            return Task.FromResult(1);
        }
    }
}
