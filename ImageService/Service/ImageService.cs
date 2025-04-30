
using ImageService.Data;
using ImageService.Models;

namespace ImageService.Service
{
    public class ImageService : IImageService
    {
        private readonly ImageDbContext _context;

        private readonly IWebHostEnvironment _env;

        public ImageService(ImageDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public async Task<string> UploadUserImageAsync(int userId, IFormFile image)
        {
            var folderPath = Path.Combine(_env.WebRootPath, "uploads/users");
            Directory.CreateDirectory(folderPath);

            var fileName = $"{userId}_{DateTime.UtcNow.Ticks}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/users/{fileName}";

            // Only one image per user, so update if exists
            var existing = await _context.UserImages.FindAsync(userId);
            if (existing != null)
            {
                existing.ImagePath = relativePath;
                existing.UploadedAt = DateTime.UtcNow;
            }
            else
            {
                await _context.UserImages.AddAsync(new UserImage
                {
                    UserId = userId,
                    ImagePath = relativePath
                });
            }

            await _context.SaveChangesAsync();
            return relativePath;
        }

        public async Task<string> UploadProductImageAsync(Guid productId, IFormFile image)
        {
            var folderPath = Path.Combine(_env.WebRootPath, "uploads/products");
            Directory.CreateDirectory(folderPath);

            var fileName = $"{productId}_{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/products/{fileName}";

            await _context.ProductImages.AddAsync(new ProductImage
            {
                ProductId = productId,
                ImagePath = relativePath
            });

            await _context.SaveChangesAsync();
            return relativePath;
        }


    }
}
