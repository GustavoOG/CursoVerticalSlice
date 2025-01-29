using Catalogo.Application.DTOs;
using Catalogo.Domain.Products;
using MediatR;

namespace Catalogo.Application.Productos.TodosLosProductos
{
    internal sealed class TodosLosProductosQueryHandler : IRequestHandler<TodosLosProductosQuery,
        List<ProductoDTO>>
    {
        private readonly IProductoRepository _productoRepository;
        public TodosLosProductosQueryHandler(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;

        }
        public async Task<List<ProductoDTO>> Handle(
            TodosLosProductosQuery request, CancellationToken cancellationToken)
        {
            var productos = await _productoRepository.GetAll(cancellationToken);
            return productos.ConvertAll(p => p.ToDTO(request.Context!));
        }
    }
}
