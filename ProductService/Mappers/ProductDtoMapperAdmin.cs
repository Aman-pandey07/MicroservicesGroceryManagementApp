using ProductService.Dto;
using ProductService.Models;

namespace ProductService.Mappers
{
    public class ProductDtoMapperAdmin
    {
        public static ProductDisplayDtoAdmin ProductDisplayMapperDtoAdmin(Product prod)
        {
            return new ProductDisplayDtoAdmin
            {
                ProductId = prod.ProductId,
                Name = prod.Name,
                Category = prod.Category,
                Description = prod.Description,
                Price = prod.Price,
                Quantity = prod.Quantity,
                CreatedAt = prod.CreatedAt
            };
        }
    }
}
