using System;
using FinancialPortfolio.APIGateway.Contracts.Orders.Enums;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging.Attributes;

namespace FinancialPortfolio.APIGateway.Contracts.Orders.Commands
{
    [Message("Orders", "Order")]
    public record CreateOrderCommand : ICommand
    {
        public OrderType Type { get; }
        
        public double Amount { get; }
        
        public decimal Price { get; }
        
        public DateTime DateTime { get; }
        
        public decimal Commission { get; }
        
        public Guid AssetId { get; }
        
        public Guid AccountId { get; }

        public CreateOrderCommand(OrderType type, double amount, decimal price, DateTime dateTime, decimal commission, Guid assetId, Guid accountId)
        {
            Type = type;
            Amount = amount;
            Price = price;
            DateTime = dateTime;
            Commission = commission;
            AssetId = assetId;
            AccountId = accountId;
        }
    }
}