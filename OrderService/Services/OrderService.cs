using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Dto;
using OrderService.Models;
using OrderService.RabbitMQ.Publisher;
using OrderService.RabbitMQ.Services;
using System.Text.Json;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public OrderService(OrderDbContext context, IHttpClientFactory httpClientFactory, IRabbitMQProducer rabbitMQProducer)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient("ProductService");
            _rabbitMQProducer = rabbitMQProducer;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto)
        {
            // Call ProductService to get product info
            var response = await _httpClient.GetAsync($"/api/Product/{dto.ProductId}");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Product not found.");

            var content = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<ProductResponseDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var order = new Order
            {
                ProductId = product.ProductId,
                ProductName = product.Name,
                Quantity = dto.Quantity,
                Price = product.Price * dto.Quantity
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderEvent = new OrderCreatedEvent
            {
                OrderId = order.OrderId.GetHashCode(),
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                Price = order.Price,
                CreatedAt = order.CreatedAt
            };

            await _rabbitMQProducer.SendOrderCreatedMessageAsync(orderEvent);

            return new OrderResponseDto
            {
                OrderId = order.OrderId,
                ProductName = order.ProductName,
                Price = order.Price,
                Quantity = order.Quantity,
                CreatedAt = order.CreatedAt
            };
        }

        public async Task<List<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return [.. orders.Select(o => new OrderResponseDto
            {
                OrderId = o.OrderId,
                ProductName = o.ProductName,
                Price = o.Price,
                Quantity = o.Quantity,
                CreatedAt = o.CreatedAt
            })];
        }
    }
}
