﻿namespace ProductService.Dto
{
    public class CreateProductDto
    {
        //public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        //public DateTime CreatedAt { get; set; }
    }
}
