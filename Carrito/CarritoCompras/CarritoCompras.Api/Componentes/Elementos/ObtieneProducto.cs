using AutoMapper;
using CarritoCompras.Api.Compartidos.Domain.Entidades;
using CarritoCompras.Api.Compartidos.Persistencia;
using CarritoCompras.Api.Compartidos.Slices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarritoCompras.Api.Componentes.CarritoCompras
{
    public sealed class ObtieneProducto : ISlice
    {
        public void AgregaEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet(
                "api/elementos/{id}",
                async (
                    [FromRoute] Guid id,
                    IMediator mediator,
                    CancellationToken cancellationToken
                    ) =>
                {
                    return await mediator.Send(new ObtieneElementoConsulta(id), cancellationToken);
                }).WithName("ObtieneElemento");
        }

        public sealed class ObtieneElementoConsulta(Guid id) : IRequest<IResult>
        {
            public Guid Id { get; } = id;
        }

        public sealed class ObtieneElementoResponse(ElementoDto elemento)
        {
            public ElementoDto Elemento { get; } = elemento;
        }

        public sealed class ObtieneElementoQueryHandler(CarritoDbContext contexto,
            IMapper mapper) : IRequestHandler<ObtieneElementoConsulta, IResult>
        {
            private readonly CarritoDbContext _contexto = contexto;

            private readonly IMapper _mapper = mapper;

            public async Task<IResult> Handle(ObtieneElementoConsulta request, CancellationToken cancellationToken)
            {
                var elementoEntidad = await _contexto.Elementos.FindAsync(request.Id, cancellationToken);
                if (elementoEntidad is null)
                {
                    return null!; // Results.NotFound();
                }

                var elemento = _mapper.Map<ElementoDto>(elementoEntidad);
                return Results.Ok(new ObtieneElementoResponse(elemento));
            }
        }

        public sealed class ObtieneElementoMapProfile : Profile
        {
            public ObtieneElementoMapProfile()
            {
                CreateMap<Elemento, ElementoDto>();
            }
        }

        public sealed class ElementoDto
        {
            public required Guid id { get; set; }

            public required string Nombre { get; set; }

            public required string Descripcion { get; set; }
        }
    }
}
