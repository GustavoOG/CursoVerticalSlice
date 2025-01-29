using Catalogo.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalogo.Application.Productos.BuscarProductos
{
    public sealed class BuscarProductosQuery : IRequest<ProductoDTO>
    {
        public HttpContext? Context { get; set; }

        public string? Code { get; set; }
    }
}
