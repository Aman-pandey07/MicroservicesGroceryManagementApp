using OrderService.RabbitMQ.Publisher;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OrderService.RabbitMQ.Services
{
    public class RabbitMQProducer : IRabbitMQProducer
    {

        public async Task SendOrderCreatedMessageAsync(OrderCreatedEvent orderEvent)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

             using var connection = await factory.CreateConnectionAsync();
             using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "orderQueue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var json = JsonSerializer. Serialize(orderEvent);
            var body = Encoding.UTF8.GetBytes(json);


            await channel.BasicPublishAsync(
                 exchange: string.Empty,
                 routingKey: "orderQueue",
                 mandatory: true,
                 basicProperties: new BasicProperties { Persistent = true},
                 body: body);

        }
    }
}
