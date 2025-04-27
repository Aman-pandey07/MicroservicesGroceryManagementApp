using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Dto;
using OrderService.Models;
using OrderService.RabbitMQ.Publisher;
using OrderService.RabbitMQ.Services;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory; // Fixed type
        private readonly IRabbitMQProducer _rabbitMQProducer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(OrderDbContext context,
            IHttpClientFactory httpClientFactory,
            IRabbitMQProducer rabbitMQProducer,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpClientFactory = httpClientFactory; // Fixed assignment
            _rabbitMQProducer = rabbitMQProducer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto)
        {
            // Step 1: Get the token from the current HTTP request
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            // Step 2: Create client
            var client = _httpClientFactory.CreateClient("ProductService"); // Fixed method call

            // Step 3: Set Bearer token if available
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            }

            // Step 4: Make the request to ProductService
            var response = await client.GetAsync($"/api/Product/InterService/{dto.ProductId}"); // Fixed usage of client
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Product not found or error: {errorContent}");
            }

            // Step 4.1: Check & Deduct Stock First
            var stockResponse = await client.GetAsync($"/api/Product/CheckStock?productId={dto.ProductId}&requestedQuantity={dto.Quantity}");
            var stockContent = await stockResponse.Content.ReadAsStringAsync();

            var stockResult = JsonSerializer.Deserialize<StockCheckResponse>(stockContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (stockResult == null || !stockResult.IsAvailable)
            {
                return new OrderResponseDto
                {
                    ErrorMessage = stockResult?.Message ?? "Insufficient stock available."
                };
            }


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
                Price = product.Price * dto.Quantity,
                UserId = GetUserIdFromToken()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderEvent = new OrderCreatedEvent
            {
                OrderId = order.OrderId.GetHashCode(),
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                Price = order.Price,
                CreatedAt = order.CreatedAt,
                
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
            return orders.Select(o => new OrderResponseDto
            {
                OrderId = o.OrderId,
                ProductName = o.ProductName,
                Price = o.Price,
                Quantity = o.Quantity,
                CreatedAt = o.CreatedAt
            }).ToList();
        }

        //this is the api response of the GetAllOrder for a user
        public async Task<List<OrderResponseDto>> GetAllOrdersUserAsync()
        {
            var userId = GetUserIdFromToken();
            // Fix: Use '==' for comparison instead of '='
            var orders = await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
            return orders.Select(o => new OrderResponseDto
            {
                OrderId = o.OrderId,
                ProductName = o.ProductName,
                Price = o.Price,
                Quantity = o.Quantity,
                CreatedAt = o.CreatedAt
            }).ToList();
        }




        private int GetUserIdFromToken()
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            if (claim == null)
                throw new Exception("User ID not found in token");

            return int.Parse(claim.Value);
        }
    }
}
