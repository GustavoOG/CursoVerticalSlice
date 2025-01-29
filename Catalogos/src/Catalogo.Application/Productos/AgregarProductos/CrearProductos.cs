using MediatR;

namespace Catalogo.Application.Productos.BuscarProductos
{
    public sealed record CrearProductos(
        string Name,
        string Description,
        decimal price,
        Guid CategoryId
        ) : IRequest<Guid>;
}
