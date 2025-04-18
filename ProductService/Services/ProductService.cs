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

        public async Task<List<ProductDisplayDto>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return products.Select(p => ProductDtoMapper.ProductDisplayMapperDto(p)).ToList();
        }

        public async Task<ProductDisplayDto> GetProductById(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return ProductDtoMapper.ProductDisplayMapperDto(product);
        }

        public async Task<bool> UpdateProduct(ProductDisplayDto dto, Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.Name = dto.Name;
            product.Category = dto.Category;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Quantity = dto.Quantity;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
