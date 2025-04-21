namespace OrderService.Dto
{
    public class CreateOrderDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
