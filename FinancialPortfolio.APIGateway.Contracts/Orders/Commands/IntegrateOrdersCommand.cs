using System;
using System.Collections.Generic;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging.Attributes;

namespace FinancialPortfolio.APIGateway.Contracts.Orders.Commands
{
    [Message("Orders", "Order")]
    public record IntegrateOrdersCommand : ICommand
    {
        public Guid AccountId { get; }
        
        public IEnumerable<IntegrateOrderCommand> Orders { get; }

        public IntegrateOrdersCommand(Guid accountId, IEnumerable<IntegrateOrderCommand> orders)
        {
            AccountId = accountId;
            Orders = orders;
        }
    }
}