using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Dto;
using OrderService.Services;

namespace OrderService.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
            Console.WriteLine("🚀 OrderController loaded");
        }


        [Authorize(Roles = "Admin,Manager,Customer,User")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            var result = await _orderService.CreateOrderAsync(dto);
            // If the result contains an error message, return a bad request with the error
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { Message = result.ErrorMessage });
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin,Manager,SuperAdmin")]
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        //[Authorize(Roles = "Admin,Manager,SuperAdmin")]
        [Authorize(Roles ="User")]
        [HttpGet("User")]
        public async Task<IActionResult> GetAllOrdersUserVersion()
        {
            var orders = await _orderService.GetAllOrdersUserAsync();
            return Ok(orders);
        }
    }
}
