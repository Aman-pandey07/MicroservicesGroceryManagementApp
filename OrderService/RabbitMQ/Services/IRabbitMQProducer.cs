using OrderService.RabbitMQ.Publisher;

namespace OrderService.RabbitMQ.Services
{
    public interface IRabbitMQProducer
    {
        Task SendOrderCreatedMessageAsync(OrderCreatedEvent order);
    }
}
