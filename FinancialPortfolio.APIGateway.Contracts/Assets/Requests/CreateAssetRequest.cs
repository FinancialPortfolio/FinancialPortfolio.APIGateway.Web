using FinancialPortfolio.APIGateway.Contracts.Assets.Enums;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Requests
{
    public record CreateAssetRequest
    {
        public string Symbol { get; set; }
        
        public string Name { get; set; }
        
        public AssetType Type { get; set; }
    }
}