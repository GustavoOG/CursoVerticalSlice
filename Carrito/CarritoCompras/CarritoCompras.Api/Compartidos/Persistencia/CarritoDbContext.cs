using CarritoCompras.Api.Compartidos.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CarritoCompras.Api.Compartidos.Persistencia
{
    public sealed class CarritoDbContext(DbContextOptions<CarritoDbContext> options) : DbContext(options)
    {
        public DbSet<Elemento> Elementos => Set<Elemento>();
        public DbSet<Carrito> Carritos => Set<Carrito>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Entidades>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.FechaCreacion = DateTime.UtcNow;
                        entry.Entity.CreadoPor = "SYSTEM";
                        entry.Entity.FechaModificacion = DateTime.UtcNow;
                        entry.Entity.ModificadoPor = "SYSTEM";
                        break;
                    case EntityState.Modified:
                        entry.Entity.FechaModificacion = DateTime.UtcNow;
                        entry.Entity.ModificadoPor = "SYSTEM";
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var carrito = Carrito.Crear("INIT_CODE", "SYSTEM");
            //var elementos = new List<Elemento> {
            //    Elemento.Crear("ITEM_CODE", "",10,15.00m, "PHONE X", "MODERNO", carrito.Id),
            //    Elemento.Crear("ITEM_AM", "",1, 13.00m,"MACBOOK", "POTENTE", carrito.Id),
            //};

            //var CarritoVacio = Carrito.Crear("INIT_PASS", "SYSTEM");
            //modelBuilder.Entity<Carrito>().HasData(carrito);
            //modelBuilder.Entity<Elemento>().HasData(elementos);
            //modelBuilder.Entity<Carrito>().HasData(CarritoVacio);


            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarritoDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
