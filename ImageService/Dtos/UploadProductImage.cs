namespace ImageService.Dtos
{
    public class UploadProductImageDto
    {
        public Guid ProductId { get; set; }
        public IFormFile ProductImage { get; set; }
    }

}
