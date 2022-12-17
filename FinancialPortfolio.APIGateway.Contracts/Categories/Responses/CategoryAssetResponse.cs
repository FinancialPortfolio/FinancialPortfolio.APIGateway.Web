using System;
using System.Collections.Generic;

namespace FinancialPortfolio.APIGateway.Contracts.Categories.Responses
{
    public class CategoryAssetResponse
    {
        public string Name { get; set; }
        
        public string Symbol { get; set; }

        public Guid AssetId { get; set; }
        
        public double ExpectedAllocationInPercentage { get; set; }

        public List<OrderResponse> Orders { get; set; }
    }
}