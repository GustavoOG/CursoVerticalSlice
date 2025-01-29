using CarritoCompras.Api;
using CarritoCompras.Api.Compartidos.Domain.Models;
using CarritoCompras.Api.Compartidos.Extensiones;
using CarritoCompras.Api.Compartidos.NetWorking.CatalogoApi;
using CarritoCompras.Api.Compartidos.Slices;
using Polly;
using Polly.Fallback;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


builder.Services.AddHttpClient<CatalogoApiServicio>((ServiceProvider, HttpClient) =>
{
    HttpClient.BaseAddress = new Uri("http://localhost:5000/");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromSeconds(5),
    };
}).SetHandlerLifetime(Timeout.InfiniteTimeSpan);

builder.Services.AddResiliencePipeline<string, IEnumerable<Catalogo>>(
    "catalogo-productos",
    pipelinebuilder =>
    {
        pipelinebuilder
        .AddFallback(new FallbackStrategyOptions<IEnumerable<Catalogo>>
        {
            FallbackAction = _
            => Outcome
            .FromResultAsValueTask<IEnumerable<Catalogo>>(
                Enumerable.Empty<Catalogo>())
        }
        );
    });

builder.Services.AddOpenApi();
builder.Services.RegisterPersistenceServices(builder.Configuration);
builder.Services.RegisterApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //app.UseSwaggerUi(options =>
    //{
    //    options.Path = "/openapi";
    //    options.DocumentPath = "/openapi/v1.json";
    //});
    //app.UseReDoc(options =>
    //{
    //    options.Path = "/openapi";
    //    options.DocumentPath = "/openapi/v1.json";
    //});
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "V1 Docuemntación de Carrito de Compras");
    });
}

//app.UseHttpsRedirection();

//Version anterior con clase estatica
//ObtieneCatalogo.AgregaEndpoint(app);
//BuscarProducto.AgregaEndpoint(app);
//ObtieneCarritoPorId.AgregaEndpoint(app);
//ObtieneCarritoPorCodigo.AgregaEndpoint(app);
//ObtieneCarritoBuscador.AgregaEndpoint(app);


app.ApplyMigration();

app.UseExceptionHandler();
app.MapSliceEndpoints();
app.Run();
