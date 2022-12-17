using System.Collections.Generic;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging.Attributes;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Commands
{
    [Message("Assets", "Asset")]
    public record FetchAssetStatisticsCommand : ICommand
    {
        public IEnumerable<string> Symbols { get; }

        public FetchAssetStatisticsCommand(IEnumerable<string> symbols)
        {
            Symbols = symbols;
        }
    }
}