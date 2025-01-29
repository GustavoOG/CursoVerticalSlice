using Catalogo.Domain.Abstractions;
using Catalogo.Domain.Products;
using MediatR;

namespace Catalogo.Application.Productos.BuscarProductos
{
    internal sealed class CrearProductosHandler : IRequestHandler<CrearProductos, Guid>
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CrearProductosHandler(IProductoRepository productoRepository, IUnitOfWork unitOfWork)
        {
            _productoRepository = productoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CrearProductos request, CancellationToken cancellationToken)
        {
            var NuevoProducto = Producto.Create(
                request.Name,
                request.price,
                request.Description,
                null,
                null,
                request.CategoryId);

            _productoRepository.Add(NuevoProducto);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return NuevoProducto.Id;

        }
    }
}
