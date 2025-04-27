namespace OrderService.Dto
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        // New property for error handling
        public string ErrorMessage { get; set; }
    }
}
