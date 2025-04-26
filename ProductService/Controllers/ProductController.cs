using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Dto;
using ProductService.Models;
using ProductService.Services;
using ProductService.Dto;

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

        [Authorize(Roles = "Admin,Manager,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {
            var createproduct = await _productService.CreateProduct(dto);
            return Ok(createproduct);
        }

        [Authorize(Roles = "Admin,Manager,SuperAdmin,User,Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        //this is only and only for inter service call from the order service 
        [Authorize(Roles = "Admin,Manager,SuperAdmin,User,Customer")]
        [HttpGet("InterService/{id}")]
        public async Task<IActionResult> GetProductByIdInterService(Guid id)
        {
            var product = await _productService.GetProductByIdInterService(id);
            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }


        //this is the admin version
        [Authorize(Roles = "Admin,Manager,SuperAdmin")]
        [HttpGet("Admin")]
        public async Task<IActionResult> GetAllProductsAdmin()
        {
            var products = await _productService.GetAllProductsAdmin();
            return Ok(products);
        }


        //this is user version
        [Authorize(Roles = "Admin,Manager,SuperAdmin,User,Customer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }


        //this is Admin Version
        [Authorize(Roles = "Admin,Manager,SuperAdmin")]
        [HttpGet("Admin/{id}")]
        public async Task<IActionResult> GetProductByIdAdmin(Guid id)
        {
            var product = await _productService.GetProductByIdAdmin(id);
            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [Authorize(Roles = "Admin,Manager,SuperAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDisplayDto dto, Guid id)
        {
            var updateResult = await _productService.UpdateProduct(dto, id);

            if (updateResult == null)
                return NotFound($"Product with ID {id} not found.");

            if (!updateResult.UpdatedFields.Any())
                return Ok($"Product with ID {id} was already up-to-date.");

            return Ok(new
            {
                Message = $"✅ Product with ID {id} updated successfully.",
                UpdatedFields = updateResult.UpdatedFields
            });
        }

        [Authorize(Roles = "Admin,Manager,SuperAdmin")]
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
