using ProductService.Dto;
using ProductService.Models;

namespace ProductService.Mappers
{
    public static class ProductDtoMapper
    {
        public static ProductDisplayDto ProductDisplayMapperDto(Product prod)
        {
            return new ProductDisplayDto
            {
                Name = prod.Name,
                Category = prod.Category,
                Description = prod.Description,
                Price = prod.Price,
                Quantity = prod.Quantity
            };
        }
    }
}
