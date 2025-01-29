using System.Globalization;
using System.Reflection;

namespace CarritoCompras.Api.Compartidos.Domain.Entidades
{
    //public class Carrito(Guid id, string codigo, string usuarioId) : Entidades(id)
    public class CarritoBuilder
    {
        private DateTime? _fechaCreacion;

        private string? _usuarioId;
        private string? _codigo;
        private ICollection<Elemento>? _elementos = new List<Elemento>();
        public static CarritoBuilder CrearNuevo()
        {
            return new CarritoBuilder();
        }

        public CarritoBuilder SetFechaCreacion(DateTime fechaCreacion)
        {
            _fechaCreacion = fechaCreacion;
            return this;
        }

        public CarritoBuilder SetUsuarioId(string usuarioId)
        {
            _usuarioId = usuarioId;
            return this;
        }


        public CarritoBuilder SetCodigo(string codigo)
        {
            _codigo = codigo;
            return this;
        }


        public CarritoBuilder AgregaElemento(Elemento elemento)
        {
            _elementos.Add(elemento);
            return this;
        }


        public Carrito Build(Guid id)
        {
            var carrito = (Carrito)Activator.CreateInstance(
                typeof(Carrito),
                BindingFlags.NonPublic | BindingFlags.Instance,
                default(Binder),
                new object[] { id, _codigo!, _usuarioId! },
                default(CultureInfo))!;
            if (carrito.FechaCreacion is null)
            {
                carrito.FechaCreacion = DateTime.UtcNow.AddHours(-6);
            }
            if (string.IsNullOrEmpty(carrito.CreadoPor))
            {
                carrito.CreadoPor = _usuarioId;
            }

            if (_elementos.Any())
            {
                carrito.Elementos = _elementos;
            }
            return carrito;
        }
    }
}
