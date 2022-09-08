using System;

namespace FinancialPortfolio.APIGateway.Contracts.Orders.Responses
{
    public record StockResponse
    {
        public Guid Id { get; set; }
        
        public string Symbol { get; set; }
        
        public string Name { get; set; }
    }
}