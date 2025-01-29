namespace CarritoCompras.Api.Compartidos.Domain.Entidades
{
    public abstract class Entidades(Guid id)
    {
        protected Entidades() : this(default) { }

        public Guid Id { get; set; } = id;

        public DateTime? FechaCreacion { get; set; }
        public string CreadoPor { get; set; } = string.Empty;

        public DateTime? FechaModificacion { get; set; }

        public string? ModificadoPor { get; set; }


    }
}
