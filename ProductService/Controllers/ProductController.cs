using Microsoft.AspNetCore.Mvc;
using ProductService.Dto;
using ProductService.Models;
using ProductService.Services;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {
            var createproduct = await _productService.CreateProduct(dto);
            return Ok(createproduct);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDisplayDto dto, Guid id)
        {
            var updated = await _productService.UpdateProduct(dto, id);
            if (!updated)
                return NotFound("Product not found to update");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var deleted = await _productService.DeleteProduct(id);
            if (!deleted)
                return NotFound("Product not found to delete");

            return NoContent();
        }
    }
}
