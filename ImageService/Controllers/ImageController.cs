using ImageService.Dtos;
using ImageService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload-user")]
        public async Task<IActionResult> UploadUserImage([FromForm] UploadUserImageDto dto)
        {
            var path = await _imageService.UploadUserImageAsync(dto.UserId, dto.UserImage);
            return Ok(new { Message = "User image uploaded", ImageUrl = path });
        }

        [HttpPost("upload-product")]
        public async Task<IActionResult> UploadProductImage([FromForm] UploadProductImageDto dto)
        {
            var path = await _imageService.UploadProductImageAsync(dto.ProductId, dto.ProductImage);
            return Ok(new { Message = "Product image uploaded", ImageUrl = path });
        }
    }
}
