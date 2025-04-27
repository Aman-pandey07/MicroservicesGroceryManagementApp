using ProductService.Dto;
using ProductService.Models;

namespace ProductService.Services
{
    public interface IProductService
    {
        Task<ProductDisplayDto> CreateProduct(CreateProductDto prod);
        Task<ProductDisplayDto> GetProductById(Guid id);
        Task<ProductDisplayDtoInterService> GetProductByIdInterService(Guid id);
        Task<ProductDisplayDtoAdmin> GetProductByIdAdmin(Guid id);
        Task<List<ProductDisplayDto>> GetAllProducts();
        Task<List<ProductDisplayDtoAdmin>> GetAllProductsAdmin();
        Task<UpdatedProductDisplayDto> UpdateProduct(ProductDisplayDto dto, Guid id);
        Task<bool> DeleteProduct(Guid id);

        Task<(bool IsAvailable, int AvailableQuantity, string Message)> CheckAndUpdateStockAsync(Guid productId, int requestedQuantity);
    }
}
