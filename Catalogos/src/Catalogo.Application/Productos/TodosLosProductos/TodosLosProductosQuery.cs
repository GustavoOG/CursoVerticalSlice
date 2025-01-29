using Catalogo.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalogo.Application.Productos.TodosLosProductos
{
    public sealed class TodosLosProductosQuery : IRequest<List<ProductoDTO>>
    {
        public HttpContext? Context { get; set; }

    }
}
