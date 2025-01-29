using CarritoCompras.Api.Compartidos.Comportamientos;
using CarritoCompras.Api.Compartidos.Data;
using CarritoCompras.Api.Compartidos.Excepciones;
using CarritoCompras.Api.Compartidos.Metricas;
using CarritoCompras.Api.Compartidos.NetWorking.CatalogoApi;
using CarritoCompras.Api.Compartidos.Persistencia;
using CarritoCompras.Api.Compartidos.Slices;
using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;

namespace CarritoCompras.Api
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            services.RegistraSlices();

            var currentAssembly = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(currentAssembly);

            services.AddMediatR(m =>
            {
                m.RegisterServicesFromAssemblies(currentAssembly)
                .AddOpenRequestPreProcessor(typeof(LogginComportamientos<>))
                .AddOpenBehavior(typeof(ModelValidationComportamientos<,>))
                .AddOpenBehavior(typeof(HandlerMatricasPerformanceComportamientos<,>));
            });
            services.AddValidatorsFromAssembly(currentAssembly);
            services.AddSingleton<HandlerMatricasPerformance>();
            services.AddScoped<ICatalogoApiCliente, CatalogoApiCliente>();
            return services;
        }

        public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("sqliteCarritoCompras") ??
                throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<CarritoDbContext>(opt =>
            {
                opt.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
                    LogLevel.Information).EnableSensitiveDataLogging();
                opt.UseSqlite(connectionString).UseSnakeCaseNamingConvention();

                opt.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));


            });

            services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
            SqlMapper.AddTypeHandler(new GuidOnlyTypeHandler());

            return services;

        }
    }
}
