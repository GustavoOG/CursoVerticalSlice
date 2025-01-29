using Bogus;
using Catalogo.Domain.Categories;
using Catalogo.Domain.Products;
using Catalogo.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Extensions
{
    public static class DataSeed
    {
        public static async Task SeedCatalogoProducto(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var service = scope.ServiceProvider;
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();
            try
            {
                var context = service.GetRequiredService<CatalogoDbContext>();
                if (!context.Set<Categoria>().Any())
                {
                    Categoria computadora = Categoria.Create("COMPUTADORA");
                    Categoria telefono = Categoria.Create("TELEFONO");
                    context.AddRange(new List<Categoria>() { computadora, telefono });
                    await context.SaveChangesAsync();
                }

                if (!context.Set<Producto>().Any())
                {
                    var computadora = await context.Set<Categoria>()
                        .Where(c => c.Name == "COMPUTADORA")
                        .FirstOrDefaultAsync();

                    var telefono = await context.Set<Categoria>()
                        .Where(c => c.Name == "TELEFONO")
                        .FirstOrDefaultAsync();

                    var faker = new Faker();
                    List<Producto> productos = new List<Producto>();
                    var defaultvalue = 10000;
                    for (var i = 1; i <= 10; i++)
                    {
                        productos.Add(Producto.Create(
                            faker.Commerce.Product(),
                            100.00m,
                            faker.Commerce.ProductDescription(),
                            $"img_{i}.jpg",
                            faker.Hashids.Encode(defaultvalue, i * 1000),
                            i > 5 ? computadora.Id : telefono.Id
                            ));
                    }
                    context.AddRange(productos);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<CatalogoDbContext>();
                logger.LogError(ex, ex.Message);
            }
        }
    }
}
