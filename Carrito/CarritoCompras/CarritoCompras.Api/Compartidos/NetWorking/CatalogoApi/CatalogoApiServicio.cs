using CarritoCompras.Api.Compartidos.Domain.Models;
using Newtonsoft.Json;

namespace CarritoCompras.Api.Compartidos.NetWorking.CatalogoApi
{
    public sealed class CatalogoApiServicio(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        private static readonly Random random = new();

        //Anterior Forma
        //public CatalogoApiServicio(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}

        public async Task<IEnumerable<Catalogo>> ObtieneProductosAsync(CancellationToken cancellationToken)
        {

            //if (random.NextDouble() < 0.6)
            //{
            //    throw new ApplicationException("La api de catálogo no esta disponible");
            //}

            var content = await _httpClient
                    .GetFromJsonAsync<IEnumerable<Catalogo>>(
                    "api/productos",
                    cancellationToken);

            return content!;
        }

        public async Task<Catalogo?> ObtieneCatalogoPorCodigoAsync(string Code, CancellationToken cancellationToken)
        {
            //Para pruebas de poly
            //if (random.NextDouble() < 0.6)
            //{
            //    throw new ApplicationException("La api de catálogo no esta disponible");
            //}

            //var content = await _httpClient
            //        .GetFromJsonAsync<Catalogo>(
            //        $"api/productos/codigo/{Code}",
            //        cancellationToken);

            var response = await _httpClient.GetAsync($"api/productos/codigo/{Code}", cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return null;
            }

            var contentString = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<Catalogo>(contentString);
            return content!;
        }
    }
}
