using System.ComponentModel.DataAnnotations;

namespace ImageService.Models
{
    public class ProductImage
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public string ImagePath { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }

}
