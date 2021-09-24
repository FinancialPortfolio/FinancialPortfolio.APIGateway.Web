namespace FinancialPortfolio.APIGateway.Contracts.Accounts.Requests
{
    public record CreateAccountRequest
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}