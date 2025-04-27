﻿namespace OrderService.Dto
{
    public class StockCheckResponse
    {
        public bool IsAvailable { get; set; }
        public int AvailableQuantity { get; set; }
        public string Message { get; set; }
    }
}
