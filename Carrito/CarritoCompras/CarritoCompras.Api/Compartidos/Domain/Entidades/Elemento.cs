namespace CarritoCompras.Api.Compartidos.Domain.Entidades
{
    public class Elemento : Entidades
    {
        private Elemento(Guid id,
                string codigo,
                string urlImagen,
                int cantidad,
                decimal precio,
                string nombre,
                string descripcion,
                Guid carritoId) : base(id)
        {

            Id = id;
            Codigo = codigo;
            UrlImagen = urlImagen;
            Cantidad = cantidad;
            Precio = precio;
            Descripcion = descripcion;
            CarritoId = carritoId;
        }

        public Elemento()
        {

        }

        public string? Codigo { get; private set; }
        public string? UrlImagen { get; private set; }
        public int Cantidad { get; private set; }
        public decimal Precio { get; private set; }
        public string? Nombre { get; private set; }
        public string? Descripcion { get; private set; }
        public Guid CarritoId { get; private set; }

        public Carrito? Carrito { get; private set; }

        public void Editar(string codigo, string urlImagen, int cantidad, decimal precio, string descripcion, string nombre)
        {
            Codigo = codigo;
            UrlImagen = urlImagen;
            Cantidad = cantidad;
            Precio = precio;
            Descripcion = descripcion;
            Nombre = nombre;
        }

        public static Elemento Crear(string codigo, string UrlImagen, int cantidad, decimal precio, string nombre, string descripcion, Guid carritoId)
        {
            return new Elemento(Guid.NewGuid(), codigo, UrlImagen, cantidad, precio, nombre, descripcion, carritoId);
        }

        public static Elemento CrearDefault(string codigo, string UrlImagen, int cantidad, decimal precio, string nombre, string descripcion)
        {
            return new Elemento(Guid.NewGuid(), codigo, UrlImagen, cantidad, precio, nombre, descripcion, default);
        }
    }
}
