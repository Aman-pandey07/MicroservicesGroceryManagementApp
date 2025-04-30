using ImageService.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageService.Data
{
    public class ImageDbContext : DbContext
    {
        public ImageDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
    }
}
