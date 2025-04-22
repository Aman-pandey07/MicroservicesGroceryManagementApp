namespace NotificationService.RabbitMQConsumer
{
    public class OrderCreatedEvent
    {

        public string OrderId { get; set; }
        public string UserEmail { get; set; }
        public decimal Amount { get; set; }
        // Add other fields as needed

    }
}
