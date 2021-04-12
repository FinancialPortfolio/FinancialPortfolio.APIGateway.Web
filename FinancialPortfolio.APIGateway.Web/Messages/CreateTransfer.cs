using FinancialPortfolio.Messaging;

namespace FinancialPortfolio.APIGateway.Web.Messages
{
    [Message("Equity", "Transfer")]
    public class CreateTransfer
    {
        public string Name { get; set; }
    }
}