using System;
using FinancialPortfolio.APIGateway.Contracts.Orders.Enums;

namespace FinancialPortfolio.APIGateway.Contracts.Orders.Responses
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        
        public OrderType Type { get; set; }
        
        public double Amount { get; set; }
        
        public decimal Price { get; set; }
        
        public DateTime DateTime { get; set; }
        
        public decimal Commission { get; set; }
        
        public StockResponse Stock { get; set; }
    }
}