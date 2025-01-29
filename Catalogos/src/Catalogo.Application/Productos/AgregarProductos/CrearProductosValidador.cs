using FluentValidation;

namespace Catalogo.Application.Productos.BuscarProductos
{
    public class CrearProductosValidador : AbstractValidator<CrearProductos>
    {
        public CrearProductosValidador()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Description).NotEmpty();
        }

    }
}
