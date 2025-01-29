using Catalogo.Domain.Products;
using Microsoft.AspNetCore.Http;

namespace Catalogo.Application.DTOs
{
    public static class ProductoMapper
    {
        public static ProductoDTO ToDTO(this Producto producto, HttpContext context)
        {
            return new ProductoDTO(
                producto.Id,
                producto.Name!,
                producto.Description!,
                producto.Price ?? 0,
                $"{context.Request.Scheme}://{context.Request.Host}/images/{producto.ImageUrl}",
                producto.Code!,
                producto.CategoryId
                );
        }
    }

    public sealed record ProductoDTO
    (
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string ImageUrl,
        string Code,
        Guid CategoriId
    );
}
