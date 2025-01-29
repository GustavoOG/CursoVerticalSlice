namespace Catalogo.Api.Controlles.Productos
{
    public sealed record ProductoRequest
    (
        string Nombre,
        string Descripcion,
        decimal Precio,
        Guid IdCategoria
    );
}
