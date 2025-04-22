namespace OrderService.RabbitMQ.Publisher
{
    public class OrderCreatedEvent
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
