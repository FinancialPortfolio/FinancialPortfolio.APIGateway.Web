using System;
using FinancialPortfolio.APIGateway.Contracts.Orders.Enums;

namespace FinancialPortfolio.APIGateway.Contracts.Orders.Commands
{
    public record IntegrateOrderCommand
    {
        public OrderType Type { get; }
        
        public double Amount { get; }
        
        public decimal Price { get; }
        
        public DateTime DateTime { get; }
        
        public decimal Commission { get; }
        
        public string Symbol { get; }

        public IntegrateOrderCommand(OrderType type, double amount, decimal price, DateTime dateTime, decimal commission, string symbol)
        {
            Type = type;
            Amount = amount;
            Price = price;
            DateTime = dateTime;
            Commission = commission;
            Symbol = symbol;
        }
    }
}