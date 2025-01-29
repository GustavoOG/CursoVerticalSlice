using AutoMapper;
using CarritoCompras.Api.Compartidos.Domain.Entidades;
using CarritoCompras.Api.Compartidos.NetWorking.CatalogoApi;
using CarritoCompras.Api.Compartidos.Persistencia;
using CarritoCompras.Api.Compartidos.Slices;
using FluentValidation;
using MediatR;

namespace CarritoCompras.Api.Componentes.Elementos
{
    public class AgregarProducto : ISlice
    {
        public void AgregaEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPost(
                "api/carritos/{carritoId}/elementos",
                async (
                    Guid carritoId,
                    AgregaElementoRequest agregaElementoRequest,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var agregarElementoCommand = new AgregaElementoCommand(
                        carritoId,
                        agregaElementoRequest.Codigo,
                        agregaElementoRequest.Cantidad
                        );
                    return await mediator.Send(agregarElementoCommand, cancellationToken);
                }
                );
        }

        public sealed class AgregaElementoRequest(string codigo, int cantidad)
        {
            public string Codigo { get; set; } = codigo;

            public int Cantidad { get; set; } = cantidad;
        }

        public sealed class AgregaElementoCommand(
            Guid carritoId, string codigoProducto, int cantidad
            ) : IRequest<IResult>
        {
            public Guid CarritoId { get; set; } = carritoId;
            public string CodigoProducto { get; set; } = codigoProducto;
            public int Cantidad { get; set; } = cantidad;
        }

        public sealed class AgregaElementoCommandValidator : AbstractValidator<AgregaElementoCommand>
        {

            public AgregaElementoCommandValidator()
            {
                RuleFor(x => x.CodigoProducto).MaximumLength(20).NotEmpty().WithMessage("Error de validación en código del producto");
                RuleFor(x => x.Cantidad).NotEmpty().WithMessage("La cantidad no puede ser 0");
            }
        }

        public sealed class AgregaElementoCommandHandler(
            ICatalogoApiCliente catalogoApiCliente,
            CarritoDbContext carritoDbContext,
            IMapper mapper
            ) : IRequestHandler<AgregaElementoCommand, IResult>
        {
            private readonly ICatalogoApiCliente _catalogoApiCliente = catalogoApiCliente;
            private readonly CarritoDbContext _carritoDbContext = carritoDbContext;
            private readonly IMapper _mapper = mapper;
            public async Task<IResult> Handle(AgregaElementoCommand request, CancellationToken cancellationToken)
            {
                //return Results.Problem(
                //    statusCode: StatusCodes.Status400BadRequest,
                //    detail: $"No se encontro el producto {request.CodigoProducto}",
                //    title: "Es un mensaje de error");

                //1. Buscar un producto del catalogo - pasando codigo como parametro
                var producto = await _catalogoApiCliente.ObtieneProductoPorcodigoAsync(request.CodigoProducto, cancellationToken);
                if (producto is null)
                {
                    //throw new Exception("No se encontro el producto en la api externa");
                    //return Results.NotFound();

                    return Results.Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: $"No se encontro el producto en catalogo: {request.CodigoProducto}",
                        title: "Es un mensaje de error");
                }

                //2. Crear objeto de tipo Elemento basado en la clase elemento item
                var elementoEntidad = Elemento.Crear(request.CodigoProducto, producto.ImageUrl!, request.Cantidad,
                    producto.Price ?? producto.Price.Value, producto.Name!, producto.Description!, request.CarritoId);

                //3. Insertar el producto en la base de datos usadno el savechangeasync
                _carritoDbContext.Elementos.Add(elementoEntidad);
                await _carritoDbContext.SaveChangesAsync(cancellationToken);

                //4. Devolver un objetro result que contenga la url que identifica el api
                //para obtener ese item por id
                return Results.Created(
                    $"api/elementos/{elementoEntidad.Id}",
                   _mapper.Map<ElementoDto>(elementoEntidad)
                    );
            }
        }

        public sealed class ElementoDto()
        {
            public required string Codigo { get; set; }
            public required string UrlImagen { get; set; }
            public required int Cantidad { get; set; }
            public required decimal Precio { get; set; }
            public required string Nombre { get; set; }
            public required string Descripcion { get; set; }
            public required Guid CarritoId { get; set; }
        }

        public sealed class ElementoMapProfileAfterCreate : Profile
        {

            public ElementoMapProfileAfterCreate()
            {
                CreateMap<Elemento, ElementoDto>();
            }
        }
    }
}
