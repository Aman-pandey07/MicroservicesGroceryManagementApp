//using Microsoft.AspNetCore.Connections;
//using Microsoft.EntityFrameworkCore.Metadata;
//using RabbitMQ.Client;

//namespace NotificationService.RabbitMQConsumer
//{
//    public class RabbitMQConsumer : BackgroundService
//    {
//        private readonly IConnection _connection;
//        private readonly IModel _channel;

//        public RabbitMQConsumer()
//        {
//            var factory = new ConnectionFactory()
//            {
//                HostName = "localhost", // RabbitMQ host
//                Port = 5672              // Default port
//            };

//            _connection = factory.CreateConnection();
//            _channel = _connection.CreateModel();

//            _channel.QueueDeclare(
//                queue: "orderQueue",     // Same name as producer queue
//                durable: false,          // False = not stored to disk
//                exclusive: false,        // Can be used by multiple connections
//                autoDelete: false,       // Won't delete itself when consumers disconnect
//                arguments: null);        // Any extra args
//        }

//        protected override Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            var consumer = new EventingBasicConsumer(_channel);

//            consumer.Received += (sender, eventArgs) =>
//            {
//                var body = eventArgs.Body.ToArray();
//                var message = Encoding.UTF8.GetString(body);

//                Console.WriteLine($"[✔] Message received in NotificationService: {message}");

//                try
//                {
//                    var orderData = JsonSerializer.Deserialize<OrderCreatedEvent>(message);

//                    // Dummy notification logic
//                    Console.WriteLine($"[✉️] Sending notification to: {orderData.UserEmail} for Order ID: {orderData.OrderId}, Amount: {orderData.Amount}");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"[❌] Error processing message: {ex.Message}");
//                }
//            };

//            // Fix: Ensure the correct method is called on the RabbitMQ channel
//            _channel.BasicConsume(
//                queue: "orderQueue",
//                autoAck: true,
//                consumer: consumer);

//            return Task.CompletedTask;
//        }

//        public override void Dispose()
//        {
//            _channel?.Close();
//            _connection?.Close();
//            base.Dispose();
//        }
//    }
//}
