namespace FinancialPortfolio.APIGateway.Contracts.Assets.Requests
{
    public record GetTransfersRequest
    {
        public int PageSize { get; set; }
        
        public int PageNumber { get; set; }
    }
}