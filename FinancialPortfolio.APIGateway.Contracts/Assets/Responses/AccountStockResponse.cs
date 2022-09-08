using System;
using System.Collections.Generic;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Responses
{
    public record AccountStockResponse
    {
        public Guid Id { get; set; }
        
        public string Symbol { get; set; }
        
        public string Name { get; set; }
        
        public string Exchange { get; set; }
        
        public List<OrderResponse> Orders { get; set; }
    }
}