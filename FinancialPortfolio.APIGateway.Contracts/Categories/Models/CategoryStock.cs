using System;

namespace FinancialPortfolio.APIGateway.Contracts.Categories.Models
{
    public record CategoryStock
    {
        public string Name { get; private set; }
        
        public string Symbol { get; private set; }

        public Guid AssetId { get; private set; }

        public CategoryStock(string name, string symbol, Guid assetId)
        {
            Name = name;
            Symbol = symbol;
            AssetId = assetId;
        }
    }
}