using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Dto;
using ProductService.Mappers;
using ProductService.Models;
using System.Reflection.Metadata.Ecma335;

namespace ProductService.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;

        public ProductService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDisplayDto> CreateProduct(CreateProductDto dto)
        {
           
            var product = CreateProductMapper.CreateProductDtoMapper(dto);

            _context.Products.Add(product);
            await _context.SaveChangesAsync(); // Save changes to the database

            return ProductDtoMapper.ProductDisplayMapperDto(product);
        }

        public Task<bool> DeleteProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        //Get All product service of the **User Version**
        public async Task<List<ProductDisplayDto>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return products.Select(p => ProductDtoMapper.ProductDisplayMapperDto(p)).ToList();
        }


        //Get All product service of the **Admin Version**
        public async Task<List<ProductDisplayDtoAdmin>> GetAllProductsAdmin()
        {
            var products = await _context.Products.ToListAsync();
            return products.Select(p => ProductDtoMapperAdmin.ProductDisplayMapperDtoAdmin(p)).ToList();
        }


        //this is the user version
        public async Task<ProductDisplayDto> GetProductById(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return ProductDtoMapper.ProductDisplayMapperDto(product);
        }

        //this is the Admin version
        public async Task<ProductDisplayDtoAdmin> GetProductByIdAdmin(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return ProductDtoMapperAdmin.ProductDisplayMapperDtoAdmin(product);
        }

        //this is only for the inter service communication for the order service which wanted the product id 
        public async Task<ProductDisplayDtoInterService> GetProductByIdInterService(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return ProductDtoMapperInterService.ProductDisplayMapperDtoInterService(product);
        }


        //Updated the UpdateProduct response
        public async Task<UpdatedProductDisplayDto> UpdateProduct(ProductDisplayDto dto, Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            var result = new UpdatedProductDisplayDto
            {
                ProductId = id
            };
            if (product.Name != dto.Name)
            {
                product.Name = dto.Name;
                result.UpdatedFields["Name"] = dto.Name;
            }

            if (product.Category != dto.Category)
            {
                product.Category = dto.Category;
                result.UpdatedFields["Category"] = dto.Category;
            }

            if (product.Description != dto.Description)
            {
                product.Description = dto.Description;
                result.UpdatedFields["Description"] = dto.Description;
            }

            if (product.Price != dto.Price)
            {
                product.Price = dto.Price;
                result.UpdatedFields["Price"] = dto.Price;
            }

            if (product.Quantity != dto.Quantity)
            {
                product.Quantity = dto.Quantity;
                result.UpdatedFields["Quantity"] = dto.Quantity;
            }

            if (result.UpdatedFields.Any())
            {
                await _context.SaveChangesAsync();
            }

            return result;
        }


        public async Task<(bool IsAvailable, int AvailableQuantity, string Message)> CheckAndUpdateStockAsync(Guid productId, int requestedQuantity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
                return (false, 0, "Product not found");

            if (product.Quantity < requestedQuantity)
                return (false, product.Quantity, $"Only {product.Quantity} items in stock");

            product.Quantity -= requestedQuantity;
            await _context.SaveChangesAsync();

            return (true, product.Quantity, "Stock is available");
        }

        public async Task<List<ProductDisplayDto>>  SearchProductsAsync(string keyword)
        {
            
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<ProductDisplayDto>();

            // Convert the search keyword to lower case to perform a case-insensitive search
            var lowerKeyword = keyword.ToLower();

            // Query products based on category, name, or description containing the keyword
            var products = await _context.Products
                .Where(p => p.Name.ToLower().Contains(lowerKeyword) ||
                            p.Category.ToLower().Contains(lowerKeyword) ||
                            p.Description.ToLower().Contains(lowerKeyword))
                .Select(p => new ProductDisplayDto
                {
                    Name = p.Name,
                    Category = p.Category,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity
                })
                .ToListAsync();

            return products;
        }


    }
}
