using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using FluentValidation;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Validation
{
    public class FetchStockStatisticsRequestValidation : AbstractValidator<FetchStockStatisticsRequest>
    {
        public FetchStockStatisticsRequestValidation()
        {
            RuleFor(x => x.Symbols).NotEmpty();
        }
    }
}