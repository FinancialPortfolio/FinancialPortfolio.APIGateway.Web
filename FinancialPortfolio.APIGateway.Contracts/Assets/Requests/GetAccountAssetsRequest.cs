namespace FinancialPortfolio.APIGateway.Contracts.Assets.Requests
{
    public record GetAccountAssetsRequest
    {
        public string Type { get; set; }
    }
}