using CarritoCompras.Api.Compartidos.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarritoCompras.Api.Compartidos.Persistencia.Configuraciones
{
    internal sealed class Elementoconfiguracion : IEntityTypeConfiguration<Elemento>
    {
        public void Configure(EntityTypeBuilder<Elemento> builder)
        {
            builder.ToTable("elementos");
            builder.HasKey(x => x.Id);
        }
    }
}
