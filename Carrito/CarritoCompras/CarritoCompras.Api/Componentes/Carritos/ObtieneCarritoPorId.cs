using AutoMapper;
using CarritoCompras.Api.Compartidos.Domain.Entidades;
using CarritoCompras.Api.Compartidos.Persistencia;
using CarritoCompras.Api.Compartidos.Slices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarritoCompras.Api.Componentes.Carritos
{
    public sealed class ObtieneCarritoPorId : ISlice
    {
        public void AgregaEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/carrito", async (
            Guid CarritoId,
                //ILoggerFactory logger,
                IMediator mediator,
                CancellationToken cancellation
                ) =>
            {
                //logger.CreateLogger("EndpoitnCarritoElementosPorId")
                //.LogInformation("Consulta de Carrito por Id");
                return await mediator.Send(new ObtieneCarritoPorIdQuery(CarritoId), cancellation);
            });
        }

        public sealed class ObtieneCarritoPorIdQuery(Guid id) : IRequest<IResult>
        {
            public Guid Id { get; } = id;
        }

        public sealed class ObtieneCarritoPorIdQueryHandler(CarritoDbContext contexto,
                IMapper mapper) : IRequestHandler<ObtieneCarritoPorIdQuery, IResult>
        {
            private readonly CarritoDbContext _contexto = contexto;
            private readonly IMapper _mapper = mapper;

            public async Task<IResult> Handle(ObtieneCarritoPorIdQuery request, CancellationToken cancellationToken)
            {
                var carrito = _contexto.Carritos
                 .Include(c => c.Elementos)
                 .Where(c => c.Id == request.Id).FirstOrDefaultAsync();

                if (carrito == null)
                {
                    Results.NotFound();
                }

                var carritoDTO = _mapper.Map<CarritoDTO>(carrito.Result);
                return Results.Ok(carritoDTO);
            }
        }

        public sealed class CarritoMapProfile : Profile
        {
            public CarritoMapProfile()
            {
                CreateMap<Carrito, CarritoDTO>();
                CreateMap<Elemento, ElementoDTO>();
            }
        }

        public sealed class CarritoDTO
        {
            public required Guid Id { get; set; }

            public required string Codigo { get; set; }

            public string? UsuarioId { get; set; }

            public List<ElementoDTO> Elementos { get; set; } = [];
        }

        public sealed class ElementoDTO
        {
            public required Guid Id { get; set; }
            public required string Codigo { get; set; }
            public string? ImagenURL { get; set; }
            public required decimal Precio { get; set; }
            public required int Cantidad { get; set; }
            public required string Nombre { get; set; }
            public required string? Descripcion { get; set; }
            public required Guid CarritoId { get; set; }


        }
    }
}
