using CarritoCompras.Api.Compartidos.Domain.Entidades;
using CarritoCompras.Api.Compartidos.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace CarritoCompras.Api.Compartidos.Extensiones
{
    public static class MigrationExtensions
    {
        public static async Task ApplyMigration(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var service = scope.ServiceProvider;
                var loggerFactory = service.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = service.GetRequiredService<CarritoDbContext>();
                    await context.Database.MigrateAsync();
                    //Agregar logica par ainsertar 
                    await EnviaDatos(context);
                }
                catch (Exception exp)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(exp, "Error en la migración");
                }
            }
        }

        public static async Task EnviaDatos(CarritoDbContext contexto)
        {

            if (!contexto.Carritos.Any())
            {
                var carritocompras = CarritoBuilder.CrearNuevo()
                        .SetFechaCreacion(DateTime.Now)
                        .SetCodigo("INIT_CODE")
                        .SetUsuarioId("SYSTEM")
                        .AgregaElemento(Elemento.CrearDefault("ITEM_CODE", "", 10, 15.00m, "Telefono XXX", "Moderno"))
                        .AgregaElemento(Elemento.CrearDefault("ITEM_AM", "", 10, 13.00m, "Telefono XXX", "Potente")
                        ).Build(Guid.NewGuid());
                contexto.Add(carritocompras);

                carritocompras = CarritoBuilder.CrearNuevo()
                        .SetFechaCreacion(DateTime.Now)
                        .SetCodigo("INIT_PASS")
                        .SetUsuarioId("SYSTEM")
                        .Build(Guid.NewGuid());
                contexto.Add(carritocompras);

                await contexto.SaveChangesAsync();

            }
        }

    }
}
