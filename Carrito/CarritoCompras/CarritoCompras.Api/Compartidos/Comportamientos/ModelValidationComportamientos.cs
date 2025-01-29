using FluentValidation;
using MediatR;

namespace CarritoCompras.Api.Compartidos.Comportamientos
{
    public sealed class ModelValidationComportamientos<TRequest, IResult>
        (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, IResult> where TRequest : IRequest<IResult>
    {

        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
        public async Task<IResult> Handle(TRequest request, RequestHandlerDelegate<IResult> next, CancellationToken cancellationToken)
        {

            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var resultadoValidaciones = _validators.Select(x => x.Validate(context)).ToList();
            var GrupoValidaciones = resultadoValidaciones
                .SelectMany(v => v.Errors)
                .GroupBy(e => e.PropertyName)
                .Select(g => new
                {
                    ProportyName = g.Key,
                    ValidationFailures = g.Select(v => new { v.ErrorMessage })
                }).ToList();
            if (GrupoValidaciones.Count != 0)
            {
                var diccionarioValidaciones = new Dictionary<string, string[]>();
                GrupoValidaciones.ForEach(grupo =>
                {
                    var mensajeError = grupo.ValidationFailures.Select(v => v.ErrorMessage);
                    diccionarioValidaciones.Add(grupo.ProportyName, mensajeError.ToArray());
                });

                return (IResult)Results.ValidationProblem(diccionarioValidaciones);

            }

            return await next();
        }
    }

}
