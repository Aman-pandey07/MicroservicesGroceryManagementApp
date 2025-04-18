using ProductService.Dto;
using ProductService.Models;

namespace ProductService.Services
{
    public interface IProductService
    {
        Task<ProductDisplayDto> CreateProduct(CreateProductDto prod);
        Task<ProductDisplayDto> GetProductById(Guid id);
        Task<List<ProductDisplayDto>> GetAllProducts();
        Task<bool> UpdateProduct(ProductDisplayDto dto, Guid id);
        Task<bool> DeleteProduct(Guid id);
    }
}
