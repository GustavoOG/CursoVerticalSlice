using Catalogo.Application.DTOs;
using Catalogo.Domain.Products;
using MediatR;

namespace Catalogo.Application.Productos.BuscarProductos
{
    internal sealed class BuscarProductosQueryHandler :
        IRequestHandler<BuscarProductosQuery, ProductoDTO>
    {
        private readonly IProductoRepository _repository;
        public BuscarProductosQueryHandler(IProductoRepository productoRepository)
        {
            _repository = productoRepository;
        }
        public async Task<ProductoDTO> Handle(BuscarProductosQuery request, CancellationToken cancellationToken)
        {
            var producto = await _repository.GetByCode(request.Code!, cancellationToken);
            return producto is null ? null! : producto!.ToDTO(request.Context!);
        }
    }
}
