namespace CarritoCompras.Api.Compartidos.Domain.Entidades
{
    //public class Carrito(Guid id, string codigo, string usuarioId) : Entidades(id)
    public class Carrito : Entidades
    {
        private Carrito()
        {
        }

        private Carrito(Guid id, string codigo, string usuarioId) : base(id)
        {
            Codigo = codigo;
            UsuarioId = usuarioId;
        }

        public string Codigo { get; private set; }
        public string UsuarioId { get; private set; }

        public ICollection<Elemento> Elementos { get; set; } = [];

        //public static Carrito Crear(string codigo, string usuariId)
        //{
        //    var carritoEntidad = new Carrito(Guid.NewGuid(), codigo, usuariId);
        //    carritoEntidad.CreadoPor = usuariId;
        //    carritoEntidad.FechaCreacion = DateTime.UtcNow.AddHours(-6);
        //    return carritoEntidad;
        //}

    }
}
