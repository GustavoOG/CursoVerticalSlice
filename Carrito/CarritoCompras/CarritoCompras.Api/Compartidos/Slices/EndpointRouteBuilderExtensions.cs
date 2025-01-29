namespace CarritoCompras.Api.Compartidos.Slices
{
    public static class EndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapSliceEndpoints
            (this IEndpointRouteBuilder endpointRouteBuilder)
        {
            foreach (ISlice slice in endpointRouteBuilder.ServiceProvider.GetServices<ISlice>())
            {
                slice.AgregaEndpoint(endpointRouteBuilder);
            }

            return endpointRouteBuilder;
        }
    }
}
