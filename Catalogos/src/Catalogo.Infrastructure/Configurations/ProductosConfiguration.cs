using Catalogo.Domain.Categories;
using Catalogo.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogo.Infrastructure.Configurations;

internal sealed class ProductosConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("productos");
        builder.HasKey(p => p.Id);

        builder.HasOne<Categoria>()
        .WithMany()
        .HasForeignKey(c => c.CategoryId);

    }
}