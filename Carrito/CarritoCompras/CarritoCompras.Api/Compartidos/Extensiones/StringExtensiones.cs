namespace CarritoCompras.Api.Compartidos.Extensiones
{
    public static class StringExtensiones
    {
        public static string EncodeForLike(this string buscador)
        {
            return buscador
                .Replace("[", "[]]")
                .Replace("%", "[%]");
        }
    }

}
