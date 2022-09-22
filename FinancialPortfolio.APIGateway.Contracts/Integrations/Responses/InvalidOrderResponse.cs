using System;

namespace FinancialPortfolio.APIGateway.Contracts.Integrations.Responses
{
    public record InvalidOrderResponse
    {
        public string Symbol { get; }
        
        public string Exchange { get; }
        
        public string Currency { get; }
        
        public DateTime DateTime { get; }
        
        public double Amount { get; }

        public InvalidOrderResponse(string symbol, string exchange, string currency, DateTime dateTime, double amount)
        {
            Symbol = symbol;
            Exchange = exchange;
            Currency = currency;
            DateTime = dateTime;
            Amount = amount;
        }
    }
}