using Catalogo.Application.Productos.BuscarProductos;
using Catalogo.Application.Productos.TodosLosProductos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalogo.Api.Controlles.Productos
{
    [Route("api/productos")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ISender _sender;

        public ProductosController(ISender sender)
        {
            _sender = sender;
        }

        // http://localhost:5000/api/productos/codigo/Qk7vCkL3
        [HttpGet("codigo/{value}")]
        public async Task<IActionResult> ObtienePorCodigo(string value)
        {
            HttpContext context = HttpContext;
            var query = new BuscarProductosQuery { Code = value, Context = context };
            var producto = await _sender.Send(query);
            return Ok(producto);
        }

        // http://localhost:5000/api/productos
        [HttpGet]
        public async Task<IActionResult> ObtieneTodos()
        {
            HttpContext context = HttpContext;
            var query = new TodosLosProductosQuery { Context = context };
            var productosDTOs = await _sender.Send(query);
            return Ok(productosDTOs);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductoRequest request)
        {
            var command = new CrearProductos(request.Nombre, request.Descripcion, request.Precio, request.IdCategoria);
            var resultado = await _sender.Send(command);
            return Ok(resultado);
        }
    }
}
