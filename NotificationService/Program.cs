using NotificationService.RabbitMQConsumer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// 🐇 RABBITMQ CONSUMER CODE
var factory = new ConnectionFactory()
{
    HostName = "localhost", // RabbitMQ server
    Port = 5672 // Default port
};

// Fix: Await the connection task to get the IConnection instance
var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

// Fix: Declare the consumer before attaching the event handler
var consumer = new AsyncEventingBasicConsumer(channel);

// Fix for CS0841: Attach the event handler after the consumer is declared
consumer.ReceivedAsync += static async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"📩 [NotificationService] Received: {message}");
    await Task.CompletedTask; // Explicitly return a completed Task to satisfy the delegate's return type.
};

await channel.QueueDeclareAsync(
    queue: "orderQueue",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

// 🔁 Set up consumer to listen
await channel.BasicConsumeAsync(
    queue: "orderQueue",
    autoAck: true,
    consumer: consumer
);

// ✅ Start web app
app.Run();