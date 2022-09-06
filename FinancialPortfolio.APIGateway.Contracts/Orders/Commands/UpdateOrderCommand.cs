using System;
using FinancialPortfolio.APIGateway.Contracts.Orders.Enums;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging.Attributes;

namespace FinancialPortfolio.APIGateway.Contracts.Orders.Commands
{
    [Message("Orders", "Order")]
    public record UpdateOrderCommand : ICommand
    {
        public Guid Id { get; }
        
        public OrderType Type { get; }
        
        public double Amount { get; }
        
        public decimal Price { get; }
        
        public DateTime DateTime { get; }
        
        public decimal Commission { get; }
        
        public Guid AssetId { get; }
        
        public Guid AccountId { get; }

        public UpdateOrderCommand(Guid id, OrderType type, double amount, decimal price, DateTime dateTime, decimal commission, Guid assetId, Guid accountId)
        {
            Id = id;
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