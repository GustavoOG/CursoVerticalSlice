using CarritoCompras.Api.Compartidos.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarritoCompras.Api.Compartidos.Persistencia.Configuraciones
{
    internal sealed class CarritoConfiguracion : IEntityTypeConfiguration<Carrito>
    {
        public void Configure(EntityTypeBuilder<Carrito> builder)
        {
            builder.ToTable("carrito_compras");
            builder.HasKey(x => x.Id);

            builder.HasMany(c => c.Elementos)
            .WithOne(c => c.Carrito)
            .HasForeignKey(c => c.CarritoId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
