using ProductService.Dto;
using ProductService.Models;

namespace ProductService.Mappers
{
    public class ProductDtoMapperInterService
    {
        public static ProductDisplayDtoInterService ProductDisplayMapperDtoInterService(Product prod)
        {
            return new ProductDisplayDtoInterService
            {
                ProductId = prod.ProductId,
                Name = prod.Name,
                Category = prod.Category,
                Description = prod.Description,
                Price = prod.Price,
                Quantity = prod.Quantity
            };
        }
    }
}
