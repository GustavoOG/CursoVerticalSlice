using CarritoCompras.Api.Compartidos.Data;
using CarritoCompras.Api.Compartidos.Extensiones;
using CarritoCompras.Api.Compartidos.Slices;
using Dapper;
using MediatR;
using System.Text;

namespace CarritoCompras.Api.Componentes.Carritos
{
    public sealed class ObtieneCarritoBuscador : ISlice
    {
        public void AgregaEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("api/carritos/{codigo}/elementos",
                async (string codigo,
                //ILoggerFactory logger,
                IMediator mediator,
                CancellationToken cancellation
                ) =>
                {
                    //logger.CreateLogger("EndpoitnCarritoBuscador")
                    //               .LogInformation("Consulta de Busqueda de Carrito por Codigo");

                    return await mediator.Send(new ObtieneCarritoBuscadorQuery(codigo), cancellation);
                });
        }


        public sealed class ObtieneCarritoBuscadorQuery(string codigo) : IRequest<IResult>
        {
            public string Codigo { get; } = codigo;
        }

        public sealed class ObtieneCarritoBuscadorQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
            : IRequestHandler<ObtieneCarritoBuscadorQuery, IResult>
        {
            private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

            public async Task<IResult> Handle(ObtieneCarritoBuscadorQuery request, CancellationToken cancellationToken)
            {
                var busqueda = "%" + request.Codigo.EncodeForLike() + "%";
                var consultawhere = $" Where a.codigo Like @Busqueda";
                var sql = new StringBuilder("""
                    select 
                        a.id as Id, 
                        a.codigo as Codigo,
                        b.id as ElementoId,
                        b.nombre as Nombre
                        from carrito_compras a 
                        left join elementos b
                        on a.id = b.carrito_id
                    """);
                sql.AppendLine(consultawhere);


                var carritodiccionario = new Dictionary<string, CarritoDTO>();
                using var coneccion = _sqlConnectionFactory.CreateConnection();
                await coneccion.QueryAsync<CarritoDTO, ElementoDTO, CarritoDTO>(
                     sql.ToString(),
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
                     new { Busqueda = busqueda },
                     splitOn: "ElementoId"
                     );
                var resultados = carritodiccionario.Values.ToList();
                //if (!resultados.Any())
                //{
                //    Results.NotFound();
                //}
                return Results.Ok(resultados);


            }
        }

        public sealed class CarritoDTO
        {
            public required Guid Id { get; set; }
            public required string Codigo { get; set; }

            public List<ElementoDTO> Elementos { get; init; } = [];
        }

        public sealed class ElementoDTO
        {
            public required Guid ElementoId { get; set; }
            public required string Nombre { get; set; }
        }
    }
}
