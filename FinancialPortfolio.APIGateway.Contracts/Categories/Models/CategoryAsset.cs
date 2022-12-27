using System;

namespace FinancialPortfolio.APIGateway.Contracts.Categories.Models
{
    public record CategoryAsset
    {
        public string Name { get; private set; }
        
        public string Symbol { get; private set; }
        
        public string Type { get; private set; }

        public Guid AssetId { get; private set; }
        
        public double ExpectedAllocationInPercentage { get; private set; }

        public CategoryAsset(string name, string symbol, string type, Guid assetId, double expectedAllocationInPercentage)
        {
            Name = name;
            Symbol = symbol;
            Type = type;
            AssetId = assetId;
            ExpectedAllocationInPercentage = expectedAllocationInPercentage;
        }
    }
}