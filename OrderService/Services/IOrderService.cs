using OrderService.Dto;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto);
        Task<List<OrderResponseDto>> GetAllOrdersAsync();
    }
}
