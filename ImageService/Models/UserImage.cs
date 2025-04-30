using System.ComponentModel.DataAnnotations;

namespace ImageService.Models
{
    public class UserImage
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string ImagePath { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }

}
