using AutoMapper;
using CarritoCompras.Api.Compartidos.Domain.Entidades;
using CarritoCompras.Api.Compartidos.NetWorking.CatalogoApi;
using CarritoCompras.Api.Compartidos.Persistencia;
using CarritoCompras.Api.Compartidos.Slices;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarritoCompras.Api.Componentes.Elementos
{
    public sealed class ActualizarProducto : ISlice
    {
        public void AgregaEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            //localhost:5001/api/elementos?id=34322
            //endpointRouteBuilder.MapPut(
            //    "api/elementos",
            //    (
            //    [FromQuery(Name = "id")] Guid id,
            //    [FromBody] ActualizaElementoRequest actualizaElementoRequest,
            //    [FromServices] IMediator mediator,
            //    [FromServices] CancellationToken cancellationToken)
            //    =>
            //    {

            //    }
            //    );


            endpointRouteBuilder.MapPut(
              "api/elementos/{id}",
              async (
              [FromRoute] Guid id,
              [FromBody] ActualizaElementoRequest actualizaElementoRequest,
              [FromServices] IMediator mediator,
              CancellationToken cancellationToken)
              =>
              {
                  var actualizaElemetocommand = new ActualizaElementoCommand(
                      id,
                      actualizaElementoRequest.CodigoProducto,
                      actualizaElementoRequest.Cantidad);
                  return await mediator.Send(actualizaElemetocommand);
              }
              );
        }

        public sealed class ActualizaElementoRequest(string codigoProducto, int cantidad)
        {
            public string CodigoProducto { get; } = codigoProducto;
            public int Cantidad { get; } = cantidad;
        }

        public sealed class ActualizaElementoCommand(Guid id, string codigoProducto, int cantidad)
            : IRequest<IResult>
        {
            public Guid Id { get; set; } = id;
            public string CodigoProducto { get; set; } = codigoProducto;
            public int Cantidad { get; set; } = cantidad;
        }

        public sealed class ActualizaElementoValidador : AbstractValidator<ActualizaElementoCommand>
        {
            public ActualizaElementoValidador()
            {
                RuleFor(m => m.Id).NotEmpty().WithMessage("No tiene ID");
                RuleFor(m => m.Cantidad).NotEmpty().WithMessage("No tiene Cantidad");
                RuleFor(m => m.CodigoProducto).NotEmpty().WithMessage("No tiene Código de producto");
            }
        }

        public sealed class ActualizaElementoCommandHandler(
            CarritoDbContext carritoDbContext,
            IMapper mapper,
            ICatalogoApiCliente catalogoApiCliente
            ) : IRequestHandler<ActualizaElementoCommand, IResult>
        {
            private readonly CarritoDbContext _carritoDbContext = carritoDbContext;
            private readonly IMapper _mapper = mapper;
            private readonly ICatalogoApiCliente _catalogoApiCliente = catalogoApiCliente;

            public async Task<IResult> Handle(
                ActualizaElementoCommand request,
                CancellationToken cancellationToken)
            {

                var producto = await _catalogoApiCliente.ObtieneProductoPorcodigoAsync(request.CodigoProducto, cancellationToken);
                if (producto is null)
                {
                    return Results.NotFound();
                }

                //var registrosafectados = await _carritoDbContext.Elementos
                //    .Where(m => m.Id == request.Id)
                //    .ExecuteUpdateAsync(x => x
                //    .SetProperty(b => b.Codigo, producto.Code)
                //    .SetProperty(b => b.Nombre, producto.Name)
                //    .SetProperty(b => b.Cantidad, request.Cantidad)
                //    .SetProperty(b => b.Descripcion, producto.Description)
                //    .SetProperty(b => b.Precio, producto.Price)
                //    .SetProperty(b => b.UrlImagen, producto.ImageUrl)
                //    );

                var elementoEntidad = await _carritoDbContext.Elementos.FindAsync(request.Id, cancellationToken);
                if (elementoEntidad is null)
                {
                    return Results.NotFound();
                }

                _carritoDbContext.Elementos.Attach(elementoEntidad);
                elementoEntidad.Editar(
                    producto.Code!,
                    producto.ImageUrl!,
                    request.Cantidad,
                    producto.Price ?? producto.Price!.Value,
                    producto.Description!,
                    producto.Name!);
                //_carritoDbContext.Entry(elementoEntidad).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                var registrosafectados = await _carritoDbContext.SaveChangesAsync(cancellationToken);

                if (registrosafectados == 0)
                {
                    Results.InternalServerError();
                }

                return Results.AcceptedAtRoute(
                    routeName: "ObtieneElemento",
                    routeValues: new { id = request.Id },
                   _mapper.Map<ElementoDto>(elementoEntidad)
                );
            }
        }

        public sealed class ElementoDto()
        {
            public required Guid Id { get; set; }
            public required string Codigo { get; set; }
            public required string Nombre { get; set; }
            public required string Descripcion { get; set; }

        }

        public sealed class ElementoMapProfileAfterUpdate : Profile
        {
            public ElementoMapProfileAfterUpdate()
            {
                CreateMap<Elemento, ElementoDto>();

            }
        }
    }
}