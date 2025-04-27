using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime OrderedAt { get; set; } = DateTime.Now;

        // Add the missing CreatedAt property to fix the error  
        public DateTime CreatedAt => OrderedAt;

        public int UserId { get; set; }
    }
}
