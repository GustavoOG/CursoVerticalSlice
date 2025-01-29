using System.Reflection;

namespace CarritoCompras.Api.Compartidos.Slices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegistraSlices(
          this IServiceCollection services)
        {

            var currentAssemby = Assembly.GetExecutingAssembly();
            var slices = currentAssemby.GetTypes()
                .Where(m => typeof(ISlice).IsAssignableFrom(m) &&
                m != typeof(ISlice) &&
                m.IsPublic &&
                !m.IsAbstract);

            foreach (var slice in slices)
            {
                services.AddSingleton(typeof(ISlice), slice);
            }

            return services;
        }
    }
}
