namespace ProductService.Dto
{
    public class ProductDisplayDtoInterService
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        //The available quantity should not be shown to the user :)
        public int Quantity { get; set; }
    }
}
