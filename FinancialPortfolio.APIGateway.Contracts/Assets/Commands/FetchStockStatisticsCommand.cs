using System.Collections.Generic;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging.Attributes;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Commands
{
    [Message("Assets", "Asset")]
    public record FetchStockStatisticsCommand : ICommand
    {
        public IEnumerable<string> Symbols { get; }

        public FetchStockStatisticsCommand(IEnumerable<string> symbols)
        {
            Symbols = symbols;
        }
    }
}