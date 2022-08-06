using System;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging.Attributes;

namespace FinancialPortfolio.APIGateway.Contracts.Accounts.Commands
{
    [Message("Accounts", "Account")]
    public record UpdateAccountCommand : ICommand
    {
        public Guid Id { get; }
        
        public string Name { get; }
        
        public string Description { get; }
        
        public Guid UserId { get; }
        
        public UpdateAccountCommand(Guid id, string name, string description, Guid userId)
        {
            Id = id;
            Name = name;
            Description = description;
            UserId = userId;
        }
    }
}