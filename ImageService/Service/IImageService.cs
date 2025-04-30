namespace ImageService.Service
{
    public interface IImageService
    {
        Task<string> UploadUserImageAsync(int userId,IFormFile image);
        Task<string> UploadProductImageAsync(Guid productId,IFormFile image);
    }
}
