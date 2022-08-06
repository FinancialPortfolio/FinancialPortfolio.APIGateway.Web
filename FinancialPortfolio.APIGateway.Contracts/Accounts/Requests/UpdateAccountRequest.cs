namespace FinancialPortfolio.APIGateway.Contracts.Accounts.Requests
{
    public record UpdateAccountRequest
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}