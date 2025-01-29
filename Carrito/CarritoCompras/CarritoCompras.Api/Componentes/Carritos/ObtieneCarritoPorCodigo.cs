using CarritoCompras.Api.Compartidos.Data;
using CarritoCompras.Api.Compartidos.Slices;
using Dapper;
using MediatR;

namespace CarritoCompras.Api.Componentes.Carritos
{
    public sealed class ObtieneCarritoPorCodigo : ISlice
    {
        public void AgregaEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/carritos/{codigo}",
                async (string codigo,
                IMediator mediator,
                //ILoggerFactory logger,
                CancellationToken cancellation
                ) =>
                {
                    //logger.CreateLogger("EndpoitnCarritoElementosPorCodigo")
                    //               .LogInformation("Consulta de Carrito por Codigo");
                    return await mediator.Send(new ObtieneCarritoPorCodigoQuery(codigo), cancellation);
                });
        }


        public sealed class ObtieneCarritoPorCodigoQuery(string codigo) : IRequest<IResult>
        {
            public string Codigo { get; } = codigo;
        }

        public sealed class ObtieneCarritoPorCodigoResponse(IEnumerable<CarritoDTO> carritos)
        {
            public IEnumerable<CarritoDTO> Carritos { get; } = carritos;
        }
        public sealed class ObtieneCarritoPorCodigoQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
            : IRequestHandler<ObtieneCarritoPorCodigoQuery, IResult>
        {
            private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;
            public async Task<IResult> Handle(ObtieneCarritoPorCodigoQuery request,
                CancellationToken cancellationToken)
            {

                const string sql = """
                    select 
                        a.id as Id, 
                        a.codigo as Codigo,
                        a.usuario_id as UsuarioId,
                        b.id as ElementoId,
                        b.nombre as Nombre,
                        b.descripcion as Descripcion
                        from carrito_compras a 
                        left join elementos b
                        on a.id = b.carrito_id
                        where a.codigo =@codigocarrito
                    """;

                var carritodiccionario = new Dictionary<string, CarritoDTO>();
                using var coneccion = _sqlConnectionFactory.CreateConnection();
                await coneccion.QueryAsync<CarritoDTO, ElementoDTO, CarritoDTO>(
                     sql,
                     (carrito, item) =>
                     {
                         if (carritodiccionario.TryGetValue(carrito.Codigo!, out var existeCarrito))
                         {
                             carrito = existeCarrito;
                         }
                         else
                         {
                             carritodiccionario.Add(carrito.Codigo, carrito);
                         }
                         carrito.Elementos.Add(item);
                         return carrito;
                     },
                     new { codigocarrito = request.Codigo },
                     splitOn: "ElementoId"
                     );

                if (carritodiccionario.Count == 0)
                    return Results.NotFound();

                var resultados = carritodiccionario.Values.ToList();
                return Results.Ok(new ObtieneCarritoPorCodigoResponse(resultados));

            }
        }

        public sealed class CarritoDTO
        {
            public required Guid Id { get; set; }
            public required string Codigo { get; set; }

            public required string UsuarioId { get; set; }

            public List<ElementoDTO> Elementos { get; init; } = [];
        }

        public sealed class ElementoDTO
        {
            public required Guid ElementoId { get; set; }
            public required string Nombre { get; set; }
            public string Descripcion { get; set; }
        }
    }
}
