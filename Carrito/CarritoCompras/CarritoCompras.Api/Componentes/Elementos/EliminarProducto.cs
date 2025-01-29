using CarritoCompras.Api.Compartidos.Persistencia;
using CarritoCompras.Api.Compartidos.Slices;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarritoCompras.Api.Componentes.Elementos
{
    public class EliminarProducto : ISlice
    {
        public void AgregaEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapDelete(
                "api/elementos/{id}",
                async (Guid id,
                IMediator mediator,
                CancellationToken candelationToken
                ) =>
                {
                    var eliminaElementoComando = new EliminaElementoCommand(id);
                    return await mediator.Send(eliminaElementoComando);
                });
        }

        public sealed class EliminaElementoCommand(Guid id) : IRequest<IResult>
        {
            public Guid Id { get; set; } = id;
        }

        public sealed class EliminaelementoCommandValidator : AbstractValidator<EliminaElementoCommand>
        {
            public EliminaelementoCommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage("El id a eliminar es necesario");
            }
        }

        public sealed class EliminaElementoCommandHandler(CarritoDbContext contexto) : IRequestHandler<EliminaElementoCommand, IResult>
        {
            private readonly CarritoDbContext _contexto = contexto;
            public async Task<IResult> Handle(EliminaElementoCommand request, CancellationToken cancellationToken)
            {
                var registrosAffectados = await _contexto.Elementos
                    .Where(m => m.Id == request.Id)
                    .ExecuteDeleteAsync(cancellationToken);
                return registrosAffectados > 0 ? Results.NoContent() : Results.NotFound();
            }
        }
    }
}
