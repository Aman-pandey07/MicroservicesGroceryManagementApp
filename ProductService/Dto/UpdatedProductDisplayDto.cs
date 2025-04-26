namespace ProductService.Dto
{
    public class UpdatedProductDisplayDto
    {
        public Guid ProductId { get; set; }
        public Dictionary<string, object> UpdatedFields { get; set; } = new();
    }
}
