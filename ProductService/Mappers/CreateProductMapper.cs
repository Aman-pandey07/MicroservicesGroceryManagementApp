using ProductService.Dto;
using ProductService.Models;

namespace ProductService.Mappers
{
    public static class CreateProductMapper
    {
        public static Product CreateProductDtoMapper(CreateProductDto prod)
        {
            return new Product
            {
                ProductId = prod.ProductId,
                Name = prod.Name,
                Category=prod.Category,
                Description = prod.Description,
                Quantity = prod.Quantity,
                Price = prod.Price,
                CreatedAt = DateTime.Now,
            };
        }
    }
}
