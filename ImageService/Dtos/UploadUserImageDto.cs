namespace ImageService.Dtos
{
    public class UploadUserImageDto
    {
        public int UserId { get; set; }
        public IFormFile UserImage { get; set; }
    }
}
